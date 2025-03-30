using FileHelpers;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using static BlazorApp.Data.Classes;

namespace BlazorApp.Data
{
	public class EmployeeService
	{
		// Employee data properties.
		public List<Employee> employeeList { get; private set; } = new();
		public Employee selectedEmployee { get; set; }
		public bool isLoading { get; private set; } = true;
		public bool recordsLoading { get; private set; } = true;

		public static Dictionary<string, string> locationNames = new();
		public string searchId { get; set; } = string.Empty;
		public string searchName { get; set; } = string.Empty;
		public List<Employee> searchResults { get; set; } = new();
		public event Action? OnEmployeesLoaded;
		public event Action? OnHL7MessagesLoaded;

		// HL7 Messages as custom Tools.Message objects.
		public List<Tools.Message> hl7Messages { get; private set; } = new();

		public async Task LoadEmployeesAsync()
		{
			if (!employeeList.Any())
			{
				employeeList = await Task.Run(() => GetEmployees());
				selectedEmployee = employeeList.First(e => e.ID == "792BDML");
			}
			isLoading = false;
			OnEmployeesLoaded?.Invoke();
		}

		public async Task LoadHL7RecordsAsync()
		{
			var time1 = DateTime.Now;
			string filePath = Path.Combine("Data", "source_hl7_messages_v2.hl7");
			if (!File.Exists(filePath))
			{
				Console.WriteLine("HL7 file not found.");
				return;
			}

			string hl7Content = await File.ReadAllTextAsync(filePath);
			ParseHL7Messages(hl7Content);
			recordsLoading = false;

			//PrintSensitiveDataDetails();

			OnHL7MessagesLoaded?.Invoke();

			Console.WriteLine($"Loaded {hl7Messages.Count} HL7 messages in {(DateTime.Now - time1).TotalMilliseconds} ms.");
		}

		private void ParseHL7Messages(string hl7Content)
		{
			int v = 0;

			var messages = Regex.Split(hl7Content, @"(?=MSH\|)");

			foreach (var message in messages.Where(m => !string.IsNullOrWhiteSpace(m)))
			{
				v++;
				string processedMessage = PreprocessMessage(message);
				try
				{
					var myMessage = ConvertHL7ToMessage(processedMessage);
					hl7Messages.Add(myMessage);
					//Console.WriteLine("Created custom Message object from HL7 message.");
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error processing HL7 message: " + ex.Message);
				}
			}
			Console.WriteLine($"Parsed {v} HL7 messages.");
		}

		private Tools.Message ConvertHL7ToMessage(string message)
		{
			var msg = new Tools.Message();

			// Map from segment identifier to tuple: (segment type, property name on Tools.Message)
			var segmentTypeMapping = new Dictionary<string, (Type SegmentType, string MessagePropName)>
	{
		{ "MSH", (typeof(Tools.MSH), "MessageHeader") },
		{ "EVN", (typeof(Tools.EVN), "EventType") },
		{ "PID", (typeof(Tools.PID), "PatientIdentification") },
		{ "PV1", (typeof(Tools.PV1), "PatientVisit") },
		{ "OBR", (typeof(Tools.OBR), "ObservationRequest") },
		{ "ORC", (typeof(Tools.ORC), "CommonOrder") },
		{ "OBX", (typeof(Tools.OBX), "ObservationResult") }
	};

			var segments = message.Split('\r');
			foreach (var segment in segments.Where(s => !string.IsNullOrWhiteSpace(s)))
			{
				var fields = segment.Split('|');
				var segType = fields[0];
				if (segmentTypeMapping.TryGetValue(segType, out var mapping))
				{
					object segmentObj = MapSegmentToObject(fields, mapping.SegmentType);
					PropertyInfo? msgProp = typeof(Tools.Message).GetProperty(mapping.MessagePropName);

					// Special handling for OBX segments: add to list rather than overwrite.
					if (segType.Equals("OBX", StringComparison.OrdinalIgnoreCase))
					{
						// Get the existing list or initialize a new one if it's null.
						var currentList = msgProp?.GetValue(msg) as IList;
						if (currentList == null)
						{
							// Create a new List<OBX> instance.
							currentList = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(mapping.SegmentType))!;
						}
						// Add the new OBX segment.
						currentList.Add(segmentObj);
						msgProp?.SetValue(msg, currentList);
					}
					else
					{
						msgProp?.SetValue(msg, segmentObj);
					}
				}
			}
			return msg;
		}


		private object MapSegmentToObject(string[] fields, Type targetType)
		{
			object segmentObj = Activator.CreateInstance(targetType)
								?? throw new InvalidOperationException("Unable to create instance of " + targetType.Name);

			var properties = targetType.GetProperties();
			for (int i = 0; i < properties.Length; i++)
			{
				int fieldIndex = i + 1;
				if (fields.Length <= fieldIndex || string.IsNullOrWhiteSpace(fields[fieldIndex]))
					continue;

				string fieldValue = fields[fieldIndex];
				PropertyInfo prop = properties[i];
				try
				{
					object? convertedValue = prop.PropertyType switch
					{
						// Existing conversions...
						Type t when t == typeof(DateTime) || t == typeof(DateTime?) => ParseHL7Date(fieldValue),
						Type t when t == typeof(int) || t == typeof(int?) => int.TryParse(fieldValue, out int intValue) ? intValue : null,
						Type t when t == typeof(long) || t == typeof(long?) => long.TryParse(fieldValue, out long longValue) ? longValue : null,
						Type t when t == typeof(char) => fieldValue[0],

						// Explicit branch for HumanizedStringList
						Type t when t == typeof(HumanizedStringList) =>
							new HumanizedStringList(fieldValue.Split('^', StringSplitOptions.RemoveEmptyEntries)),

						//// Explicit branch for List<string>
						//Type t when t == typeof(List<string>) =>
						//	fieldValue.Split('^', StringSplitOptions.RemoveEmptyEntries).ToList(),

						Type t when t == typeof(List<DateTime>) =>
							fieldValue.Split('^', StringSplitOptions.RemoveEmptyEntries)
									  .Select(ParseHL7Date)
									  .Where(d => d.HasValue)
									  .Select(d => d.Value)
									  .ToList(),

						// Custom type that provides its own parsing from a list of strings
						Type t when HasFromDelimitedFieldsMethod(t) =>
							InvokeFromDelimitedFields(t, fieldValue.Split('^', StringSplitOptions.RemoveEmptyEntries).ToList()),

						// Fallback for any other type
						_ => Convert.ChangeType(fieldValue, prop.PropertyType)
					};

					prop.SetValue(segmentObj, convertedValue);
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error mapping field '{fieldValue}' to property '{prop.Name}' of type '{prop.PropertyType.Name}': {ex.Message}");
				}
			}
			return segmentObj;
		}

		private static bool HasFromDelimitedFieldsMethod(Type t)
		{
			return t.GetMethod("FromDelimitedFields", BindingFlags.Public | BindingFlags.Static) != null;
		}

		private static object InvokeFromDelimitedFields(Type t, List<string> fields)
		{
			var method = t.GetMethod("FromDelimitedFields", BindingFlags.Public | BindingFlags.Static);
			if (method == null)
				throw new InvalidOperationException($"Type {t.Name} does not implement FromDelimitedFields method.");

			return method.Invoke(null, new object[] { fields });
		}





		public static DateTime? ParseHL7Date(string hl7Date)
		{
			if (string.IsNullOrWhiteSpace(hl7Date))
				return null;

			// Define a comprehensive list of possible date formats.
			string[] formats = new[]
			{
				"yyyyMMdd",
				"yyyyMMddHHmmss",
				"yyyyMMddHHmm",
				"yyyy-MM-dd",
				"yyyy-MM-ddTHH:mm:ss",
				"MM/dd/yyyy",
				"MM/dd/yyyy HH:mm:ss",
				"dd-MMM-yyyy",
				"dd/MM/yyyy",
				"dd.MM.yyyy",
				"M/d/yyyy",
				"M/d/yyyy h:mm:ss tt",
				"yyyyMMddHHmmzzz"
			    // Add more formats as needed.
			};

			// First, try parsing with the defined formats.
			if (DateTime.TryParseExact(hl7Date, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedDate))
				return parsedDate;

			// Fallback: attempt to parse using DateTime.TryParse, which handles many common formats.
			if (DateTime.TryParse(hl7Date, out parsedDate))
				return parsedDate;

			// Return null if parsing fails.
			return null;
		}


		private string PreprocessMessage(string message)
		{
			// Define allowed OBX-2 values.
			var allowedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
			{
				"ST", "NM", "TX", "FT", "CE", "DT", "TM", "TS", "ID", "SI"
			};

			var lines = message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].StartsWith("MSH") && lines[i].Length > 3)
				{
					char fieldSeparator = lines[i][3];
					var fields = lines[i].Substring(4).Split(fieldSeparator).ToList();
					if (fields.Any() && (fields[0].Length != 4 || fields[0].Distinct().Count() != 4))
						fields[0] = "^~\\&";
					lines[i] = "MSH" + fieldSeparator + string.Join(fieldSeparator.ToString(), fields);
				}
				else if (lines[i].StartsWith("OBX|"))
				{
					var fields = lines[i].Split('|').ToList();
					if (fields.Count >= 6 && !string.IsNullOrWhiteSpace(fields[5]))
					{
						if (string.IsNullOrWhiteSpace(fields[2]) || !allowedTypes.Contains(fields[2].Trim()))
							fields[2] = "ST";
					}
					lines[i] = string.Join("|", fields);
				}
			}
			return string.Join("\r", lines);
		}

		public void SearchEmployees()
		{
			searchResults = employeeList
				.Where(employee =>
					employee.ID.Contains(searchId, StringComparison.OrdinalIgnoreCase) &&
					employee.Name.Contains(searchName, StringComparison.OrdinalIgnoreCase))
				.ToList();
		}

		private static List<Employee> GetEmployees()
		{
			var engine = new FileHelperEngine<CSVEmployee> { Options = { IgnoreFirstLines = 1 } };
			// Filter out potential null records directly.
			var records = engine.ReadFile(Path.Combine("Data", "orgchart_faux.csv")).Where(r => r != null);

			var employees = new List<Employee>();
			foreach (var record in records)
			{
				var employee = new Employee
				{
					ID = record.Emp34Id,
					Name = $"{record.EmpFirstName} {record.EmpLastName}",
					Email = record.EmpEmailAddress,
					Position = record.EmpPositionDesc,
					Location = record.EmpLocationCode,
					Anniversary = string.IsNullOrEmpty(record.EmpAnnivDate) ? "Null" : record.EmpAnnivDate,
					Up = null,
					Downs = new List<Employee>()
				};
				employees.Add(employee);
				if (!locationNames.ContainsKey(record.EmpLocationCode))
					locationNames.Add(record.EmpLocationCode, record.EmpLocationDesc);
			}

			var employeeDict = employees.ToDictionary(e => e.ID);
			foreach (var record in records)
			{
				if (!string.IsNullOrEmpty(record.Mgr34Id) &&
					employeeDict.TryGetValue(record.Emp34Id, out var employee) &&
					employeeDict.TryGetValue(record.Mgr34Id, out var manager))
				{
					employee.Up = manager;
					manager.Downs.Add(employee);
				}
			}
			return employees;
		}

		public void PrintSensitiveDataDetails()
		{
			//List<object> redactedPatients = new();

			if (hl7Messages == null || !hl7Messages.Any())
			{
				return; // Exit if there are no HL7 messages
			}

			foreach (Tools.Message msg in hl7Messages)
			{
				if (msg.PatientIdentification != null)
				{
					string extractedEmail = ""; // Initialize extractedEmail here

					// Check if ObservationResult is not null and contains values
					if (msg.ObservationResult != null && msg.ObservationResult.Any())
					{
						foreach (var obx in msg.ObservationResult)
						{
							if (obx.ObservationValue != null && obx.ObservationValue is List<string> observationValues)
							{
								// Iterate through each string in the observation value list
								foreach (var observationValue in observationValues)
								{
									if (!string.IsNullOrEmpty(observationValue))
									{
										// Use a regular expression to extract a valid email from the string
										var emailMatch = Regex.Match(observationValue, @"\b[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}\b");

										if (emailMatch.Success)
										{
											extractedEmail = emailMatch.Value; // Extract the first valid email
											break; // Stop after finding the first email
										}
									}
								}
							}

							// If email is found, stop further searching in the ObservationResult
							if (!string.IsNullOrEmpty(extractedEmail))
							{
								break;
							}
						}
					}
					if (msg.PatientIdentification.PatientName != null)
					{
						msg.PatientIdentification.PatientName.GivenName = "********REDACTED********"; // Redact given name
						msg.PatientIdentification.PatientName.FamilyName = "********REDACTED********"; // Redact family name
					}
					if (msg.PatientIdentification.DateTimeOfBirth != null)
					{
						msg.PatientIdentification.DateTimeOfBirth = DateTime.MinValue; // Redact date of birth
					}
					if (msg.PatientIdentification.PatientAddress != null)
					{
						msg.PatientIdentification.PatientAddress.StreetAddress = "********REDACTED********"; // Redact street address
					}
					if (msg.PatientIdentification.HomePhoneNumbers != null)
					{
						msg.PatientIdentification.HomePhoneNumbers.TelephoneNumber = "********REDACTED********"; // Redact home phone number
					}
					if (msg.PatientIdentification.BusinessPhoneNumbers != null)
					{
						msg.PatientIdentification.BusinessPhoneNumbers.TelephoneNumber = "********REDACTED********"; // Redact business phone number
					}
					if (msg.PatientIdentification.SSNNumber != null)
					{
						msg.PatientIdentification.SSNNumber = "********REDACTED********"; // Redact SSN
					}
					if (msg.PatientIdentification.PatientIdentifierList != null)
					{
						msg.PatientIdentification.PatientIdentifierList.IdNumber = "********REDACTED********"; // Redact patient identifier
					}
					if (msg.PatientIdentification.PatientAccountNumber != null)
					{
						msg.PatientIdentification.PatientAccountNumber = "********REDACTED********"; // Redact account number
					}



					// Get the lengths of the patient data attributes
					int nameLength = msg.PatientIdentification.PatientName?.GivenName?.Length ?? 0; // Redact given name
					int dobLength = msg.PatientIdentification.DateTimeOfBirth?.ToString("yyyyMMdd").Length ?? 0; // Use date string length for DOB
					int addressLength = msg.PatientIdentification.PatientAddress?.StreetAddress?.Length ?? 0; // Redact street address
					int homePhoneLength = msg.PatientIdentification.HomePhoneNumbers?.TelephoneNumber?.Length ?? 0; // Redact phone number
					int businessPhoneLength = msg.PatientIdentification.BusinessPhoneNumbers?.TelephoneNumber?.Length ?? 0; // Redact business phone number
					int ssnLength = msg.PatientIdentification.SSNNumber?.Length ?? 0;
					int emailLength = extractedEmail.Length;
					int mrnLength = msg.PatientIdentification.PatientIdentifierList?.IdNumber?.Length ?? 0; // Redact patient identifier
					int accountNumberLength = msg.PatientIdentification.PatientAccountNumber?.Length ?? 0;

					// Create a dynamic object to allow modification of properties
					dynamic redactedPatientInfo = new
					{
						Names = new string('*', nameLength), // Redacted names
						DateOfBirth = new string('*', dobLength), // Redacted Date of Birth
						Addresses = new string('*', addressLength), // Redacted addresses
						HomePhones = new string('*', homePhoneLength), // Redacted home phone numbers
						BusinessPhones = new string('*', businessPhoneLength), // Redacted business phone numbers
						SSN = new string('*', ssnLength), // Redacted SSN
						Email = new string('*', emailLength), // Redacted email with the length of the original email
						MRNs = new string('*', mrnLength), // Redacted MRNs
						PatientAccountNumber = new string('*', accountNumberLength), // Redacted account number
						Age = new string('*', 5) // Redacted age as asterisks (you can adjust logic here if needed)
					};

					//redactedPatients.Add(redactedPatientInfo);
				}
				//else
				//{
				//	Console.WriteLine("PatientIdentification is null.");
				//}
			}

			//// Console log to output the redacted patient data
			//if (redactedPatients.Any())
			//{
			//	Console.WriteLine("Redacted Patient Data:");
			//	foreach (var patient in redactedPatients)
			//	{
			//		Console.WriteLine($"Name(s): {((dynamic)patient).Names}");
			//		Console.WriteLine($"Birthday: {((dynamic)patient).DateOfBirth}");
			//		Console.WriteLine($"Age: {((dynamic)patient).Age}");
			//		Console.WriteLine($"Address: {((dynamic)patient).Addresses}");
			//		Console.WriteLine($"Home Phone(s): {((dynamic)patient).HomePhones}");
			//		Console.WriteLine($"Business Phone(s): {((dynamic)patient).BusinessPhones}");
			//		Console.WriteLine($"SSN: {((dynamic)patient).SSN}");
			//		Console.WriteLine($"Email: {((dynamic)patient).Email}");
			//		Console.WriteLine($"MRN(s): {((dynamic)patient).MRNs}");
			//		Console.WriteLine($"Account Number: {((dynamic)patient).PatientAccountNumber}");
			//		Console.WriteLine($"{((dynamic)patient).ToString()}");
			//		Console.WriteLine("----------------------------");
			//	}
			//}
			//else
			//{
			//	Console.WriteLine("No patients with extracted emails.");
			//}
		}

	}
}
