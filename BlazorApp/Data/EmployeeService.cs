using FileHelpers;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Humanizer;
using Microsoft.VisualBasic;
using NHapi.Model.V22.Datatype;
using edu.stanford.nlp.pipeline;
using System.IO;
using static BlazorApp.Data.Classes;
using edu.stanford.nlp.util;
using edu.stanford.nlp.ling;

namespace BlazorApp.Data
{
	public class EmployeeService
	{
		// Properly initialize the dictionary
		public static Dictionary<string, Tools.Message> EmployeeRemap { get; private set; } =
			new Dictionary<string, Tools.Message>();

		// List of remapped messages
		public List<Tools.Message> DImessages { get; private set; } = new List<Tools.Message>();

		// Assuming hl7Messages are passed into this service. Adjust the type as needed.
		//private IEnumerable<Tools.Message> hl7Messages;

		public void DILoader()
		{
			// Load the remap file
			string filePath = Path.Combine("Data", "remap.json");
			if (!File.Exists(filePath))
			{
				Console.WriteLine("Remap file not found.");
				// Proceed with an empty remap dictionary if desired.
			}
			else
			{
				try
				{
					string json = File.ReadAllText(filePath);
					var remapFromFile = JsonSerializer.Deserialize<Dictionary<string, Tools.Message>>(json);
					if (remapFromFile != null)
					{
						EmployeeRemap = remapFromFile;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error reading remap file: " + ex.Message);
				}
			}

			// Process each HL7 message and remap as needed.
			foreach (var message in hl7Messages)
			{
				string? ID = null;
				if (message.PatientIdentification?.PatientAccountNumber != null)
				{
					ID = message.PatientIdentification.PatientAccountNumber;
				}
				else if (message.PatientIdentification?.PatientIdentifierList != null)
				{
					ID = message.PatientIdentification.PatientIdentifierList.IdNumber;
				}
				if (ID == null)
					throw new Exception("ID is null");

				// Check if remap for this message already exists.
				if (EmployeeRemap.TryGetValue(ID, out Tools.Message existingRemap))
				{
					// Remap using the existing dictionary value.
					Tools.Message remappedMessage = RemapMessage(message, existingRemap);
					DImessages.Add(remappedMessage);
				}
				else
				{
					// Create a new remap mapping for this message.
					Tools.Message newRemap = CreateRemap(message);
					// Store the new mapping.
					EmployeeRemap[ID] = newRemap;
					// Remap the message with the new mapping.
					Tools.Message remappedMessage = RemapMessage(message, newRemap);
					DImessages.Add(remappedMessage);
				}
			}
			SaveRemapToFile();
		}

		/// <summary>
		/// Remaps the original message using the provided remap fields.
		/// Replace the below stub with your actual remapping logic.
		/// </summary>
		private Tools.Message RemapMessage(Tools.Message original, Tools.Message remapFields)
		{
			// For demonstration, we assume a shallow copy.
			Tools.Message newMessage = original;

			if (newMessage.PatientIdentification?.PatientName != null)
			{
				newMessage.PatientIdentification.PatientName.GivenName =
					remapFields.PatientIdentification.PatientName.GivenName; // Redact given name
				newMessage.PatientIdentification.PatientName.FamilyName =
					remapFields.PatientIdentification.PatientName.FamilyName; // Redact family name
			}
			if (newMessage.PatientIdentification?.DateTimeOfBirth != null)
			{
				newMessage.PatientIdentification.DateTimeOfBirth =
					remapFields.PatientIdentification.DateTimeOfBirth; // Redact date of birth as needed
			}
			if (newMessage.PatientIdentification?.PatientAddress != null)
			{
				newMessage.PatientIdentification.PatientAddress.StreetAddress =
					remapFields.PatientIdentification.PatientAddress.StreetAddress; // Redact address as needed
			}
			if (newMessage.PatientIdentification?.SSNNumber != null)
			{
				newMessage.PatientIdentification.SSNNumber =
					remapFields.PatientIdentification.SSNNumber; // Redact SSN
			}
			if (newMessage.PatientIdentification?.PatientIdentifierList != null)
			{
				newMessage.PatientIdentification.PatientIdentifierList.IdNumber =
					remapFields.PatientIdentification.PatientIdentifierList.IdNumber; // Redact identifier
			}
			if (newMessage.PatientIdentification?.PatientAccountNumber != null)
			{
				newMessage.PatientIdentification.PatientAccountNumber =
					remapFields.PatientIdentification.PatientAccountNumber; // Redact account
			}

			if (newMessage.ObservationResult != null && newMessage.ObservationResult.Count > 0)
			{
				foreach (var obx in newMessage.ObservationResult)
				{
					if (obx.ObservationIdentifier != null)
					{
						obx.ObservationIdentifier.Identifier = GenerateFakeData("id");
					}
					if (obx.ObservationValue != null)
					{
						var redactedValues = new HumanizedStringList();
						foreach (var item in obx.ObservationValue)
						{
							redactedValues.Add(GenerateFakeData("default", item.Length));
						}
						obx.ObservationValue = redactedValues;
					}
				}
			}

			return newMessage;
		}



		private static StanfordCoreNLP pipeline;


		public static void InitializeRedactifyProcessor()
		{
			//var props = new java.util.Properties();
			//props.put("annotators", "tokenize,ssplit,pos,lemma,ner");
			//props.put("ner.model", "path/to/english.all.3class.distsim.crf.ser.gz"); // Ensure the model path is correct
			//props.put("ner.model", Path.Combine("Data", "source_hl7_messages_v2.hl7"));
			//pipeline = new StanfordCoreNLP(props);
		}

		private static string Redactify(string input)
		{
			return input;
			if (string.IsNullOrEmpty(input)) return input;

			var annotation = new Annotation(input);
			pipeline.annotate(annotation);

			var sentences = annotation.get(typeof(List<CoreMap>)) as List<CoreMap>;
			if (sentences != null)
			{
				foreach (var sentence in sentences)
				{
					var tokens = sentence.get(typeof(List<CoreLabel>)) as List<CoreLabel>;
					if (tokens != null)
					{
						foreach (var token in tokens)
						{
							string word = token.get(typeof(CoreAnnotations.TextAnnotation)) as string;
							string ner = token.get(typeof(CoreAnnotations.NamedEntityTagAnnotation)) as string;

							if (ner == "PERSON" || ner == "DATE")
							{
								input = input.Replace(word, new string('*', word.Length));
							}
						}
					}
				}
			}
			return input;
		}







		public Tools.Message CreateRemap(Tools.Message message)
		{
			Tools.Message newRemap = new Tools.Message();

			// Process PatientIdentification (PID segment)
			if (message.PatientIdentification != null)
			{
				newRemap.PatientIdentification = new Tools.PID
				{
					// For required PatientName, use fake names.
					PatientName = message.PatientIdentification.PatientName != null
						? new XPN
						{
							GivenName = !string.IsNullOrEmpty(message.PatientIdentification.PatientName.GivenName)
								? GenerateFakeData("firstname")
								: "redacted",
							FamilyName = !string.IsNullOrEmpty(message.PatientIdentification.PatientName.FamilyName)
								? GenerateFakeData("lastname")
								: "redacted"
						}
						: new XPN { GivenName = "redacted", FamilyName = "redacted" },

					// For required PatientIdentifierList, use fake ID.
					PatientIdentifierList = message.PatientIdentification.PatientIdentifierList != null
						? new CX
						{
							IdNumber = !string.IsNullOrEmpty(message.PatientIdentification.PatientIdentifierList.IdNumber)
								? GenerateFakeData("id")
								: "redacted"
						}
						: new CX { IdNumber = "redacted" },

					// Copy DateTimeOfBirth
					DateTimeOfBirth = message.PatientIdentification.DateTimeOfBirth,

					// Optional fields:
					PatientAccountNumber = !string.IsNullOrEmpty(message.PatientIdentification.PatientAccountNumber)
						? GenerateFakeData("account")
						: null,
					SSNNumber = !string.IsNullOrEmpty(message.PatientIdentification.SSNNumber)
						? GenerateFakeData("ssn")
						: null
				};

				// Process additional PID fields if desired.
				if (message.PatientIdentification.PatientAddress != null)
				{
					newRemap.PatientIdentification.PatientAddress = new XAD
					{
						StreetAddress = !string.IsNullOrEmpty(message.PatientIdentification.PatientAddress.StreetAddress)
							? GenerateFakeData("address")
							: null
					};
				}
				if (message.PatientIdentification.HomePhoneNumbers != null)
				{
					newRemap.PatientIdentification.HomePhoneNumbers = new XTN
					{
						TelephoneNumber = !string.IsNullOrEmpty(message.PatientIdentification.HomePhoneNumbers.TelephoneNumber)
							? GenerateFakeData("phone")
							: null
					};
				}
				if (message.PatientIdentification.BusinessPhoneNumbers != null)
				{
					newRemap.PatientIdentification.BusinessPhoneNumbers = new XTN
					{
						TelephoneNumber = !string.IsNullOrEmpty(message.PatientIdentification.BusinessPhoneNumbers.TelephoneNumber)
							? GenerateFakeData("phone")
							: null
					};
				}
			}

			// Process ObservationResult (OBX segments)
			if (message.ObservationResult != null && message.ObservationResult.Count > 0)
			{
				newRemap.ObservationResult = new List<Tools.OBX>();
				foreach (var obx in message.ObservationResult)
				{
					Tools.OBX newObx = new Tools.OBX
					{
						ObservationIdentifier = obx.ObservationIdentifier != null
							? new Classes.CE
							{
								Identifier = !string.IsNullOrEmpty(obx.ObservationIdentifier.Identifier)
									? GenerateFakeData("id")
									: "redacted"
							}
							: new Classes.CE { Identifier = "redacted" },
						ObservationResultStatus = !string.IsNullOrEmpty(obx.ObservationResultStatus)
							? GenerateFakeData("default", obx.ObservationResultStatus.Length)
							: "redacted"
					};

					if (obx.ObservationValue != null)
					{
						HumanizedStringList newValues = new HumanizedStringList();
						foreach (var val in obx.ObservationValue)
						{
							newValues.Add(!string.IsNullOrEmpty(val)
								? GenerateFakeData("default", val.Length)
								: "redacted");
						}
						newObx.ObservationValue = newValues;
					}
					else
					{
						newObx.ObservationValue = new HumanizedStringList();
					}

					// Copy over other non-sensitive fields if needed.
					newObx.SetID = obx.SetID;
					newObx.ValueType = obx.ValueType;
					newObx.ObservationSubID = obx.ObservationSubID;
					newObx.Units = obx.Units;
					newObx.ReferencesRange = obx.ReferencesRange;
					newObx.AbnormalFlags = obx.AbnormalFlags;
					newObx.Probability = obx.Probability;
					newObx.NatureOfAbnormalTest = obx.NatureOfAbnormalTest;
					newObx.EffectiveDateOfReferenceRange = obx.EffectiveDateOfReferenceRange;
					newObx.UserDefinedAccessChecks = obx.UserDefinedAccessChecks;
					newObx.DateTimeOfObservation = obx.DateTimeOfObservation;
					newObx.ProducersID = obx.ProducersID;
					newObx.ResponsibleObserver = obx.ResponsibleObserver;
					newObx.ObservationMethod = obx.ObservationMethod;
					newObx.EquipmentInstanceIdentifier = obx.EquipmentInstanceIdentifier;
					newObx.DateTimeOfAnalysis = obx.DateTimeOfAnalysis;

					newRemap.ObservationResult.Add(newObx);
				}
			}

			return newRemap;
		}

		/// <summary>
		/// Generates a random string of the specified length using letters and digits.
		/// </summary>
		private static Random random = new Random();

		private static string GenerateRandomString(int length)
		{
			if (length <= 0)
				return string.Empty;

			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			return new string(Enumerable.Repeat(chars, length)
				.Select(s => s[random.Next(s.Length)]).ToArray());
		}


		private static string GenerateFakeData(string type, int length = 0)
		{
			// Predefined lists for fake data
			string[] firstNames = { "John", "Jane", "Alice", "Bob", "Charlie", "David", "Emma", "Olivia", "Liam", "Sophia" };
			string[] lastNames = { "Smith", "Johnson", "Brown", "Williams", "Jones", "Miller", "Davis", "Wilson", "Anderson", "Taylor" };
			string[] streetNames = { "Maple St.", "Oak Ave.", "Pine Rd.", "Main St.", "Cedar Ln.", "Elm Dr." };

			switch (type.ToLower())
			{
				case "name":
					return $"{firstNames[random.Next(firstNames.Length)]} {lastNames[random.Next(lastNames.Length)]}";

				case "firstname":
					return firstNames[random.Next(firstNames.Length)];

				case "lastname":
					return lastNames[random.Next(lastNames.Length)];

				case "id":
				case "ssn":
				case "account":
					// Generate a 9-digit fake number
					return random.Next(100000000, 999999999).ToString();

				case "address":
					return $"{random.Next(100, 9999)} {streetNames[random.Next(streetNames.Length)]}, City, ST 12345";

				case "phone":
					return $"({random.Next(200, 999)}) {random.Next(100, 999)}-{random.Next(1000, 9999)}";

				default:
					// For other fields, generate a random alphabetic string
					const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
					int len = length > 0 ? length : 8;
					return new string(Enumerable.Repeat(chars, len)
						.Select(s => s[random.Next(s.Length)]).ToArray());
			}
		}














































		public bool recordsLoading { get; private set; } = true;

		//public static Dictionary<string, string> locationNames = new();
		public event Action? OnHL7MessagesLoaded;

		// HL7 Messages as custom Tools.Message objects.
		public List<Tools.Message> hl7Messages { get; private set; } = new();

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

			DILoader();

			//PrintSensitiveDataDetails();

			//WriteDataToFile();

			//DeIdentifier();
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
		public void SaveRemapToFile()
		{
			string filePath = Path.Combine("Data", "remap.json");
			try
			{
				string json = JsonSerializer.Serialize(EmployeeRemap);
				File.WriteAllText(filePath, json);
				Console.WriteLine("Remap file saved successfully.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error saving remap file: " + ex.Message);
			}
		}

		public void PrintSensitiveDataDetails()
		{
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

					// Redact sensitive data in PatientIdentification
					if (msg.PatientIdentification.PatientName != null)
					{
						msg.PatientIdentification.PatientName.GivenName = "****************"; // Redact given name
						msg.PatientIdentification.PatientName.FamilyName = "****************"; // Redact family name
					}
					if (msg.PatientIdentification.DateTimeOfBirth != null)
					{
						msg.PatientIdentification.DateTimeOfBirth = DateTime.MinValue; // Redact date of birth
					}
					if (msg.PatientIdentification.PatientAddress != null)
					{
						msg.PatientIdentification.PatientAddress.StreetAddress = "****************"; // Redact street address
					}
					if (msg.PatientIdentification.HomePhoneNumbers != null)
					{
						msg.PatientIdentification.HomePhoneNumbers.TelephoneNumber = "****************"; // Redact home phone number
					}
					if (msg.PatientIdentification.BusinessPhoneNumbers != null)
					{
						msg.PatientIdentification.BusinessPhoneNumbers.TelephoneNumber = "****************"; // Redact business phone number
					}
					if (msg.PatientIdentification.SSNNumber != null)
					{
						msg.PatientIdentification.SSNNumber = "****************"; // Redact SSN
					}
					if (msg.PatientIdentification.PatientIdentifierList != null)
					{
						msg.PatientIdentification.PatientIdentifierList.IdNumber = "****************"; // Redact patient identifier
					}
					if (msg.PatientIdentification.PatientAccountNumber != null)
					{
						msg.PatientIdentification.PatientAccountNumber = "****************"; // Redact account number
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

					// Print the redacted patient data to the console
					Console.WriteLine("Redacted Patient Data:");
					Console.WriteLine($"Name(s): {((dynamic)redactedPatientInfo).Names}");
					Console.WriteLine($"Birthday: {((dynamic)redactedPatientInfo).DateOfBirth}");
					Console.WriteLine($"Age: {((dynamic)redactedPatientInfo).Age}");
					Console.WriteLine($"Address: {((dynamic)redactedPatientInfo).Addresses}");
					Console.WriteLine($"Home Phone(s): {((dynamic)redactedPatientInfo).HomePhones}");
					Console.WriteLine($"Business Phone(s): {((dynamic)redactedPatientInfo).BusinessPhones}");
					Console.WriteLine($"SSN: {((dynamic)redactedPatientInfo).SSN}");
					Console.WriteLine($"Email: {((dynamic)redactedPatientInfo).Email}");
					Console.WriteLine($"MRN(s): {((dynamic)redactedPatientInfo).MRNs}");
					Console.WriteLine($"Account Number: {((dynamic)redactedPatientInfo).PatientAccountNumber}");
					Console.WriteLine("----------------------------");
				}
			}
		}


		public void WriteDataToFile()
		{
			string path = Path.Combine("Data", "messages_redacted.txt");

			// Initialize StreamWriter to write to the file
			using (StreamWriter outfile = new StreamWriter(path))
			{
				foreach (Tools.Message msg in hl7Messages)
				{
					// Write the humanized version of each message to the file
					outfile.WriteLine(msg.ToString());
				}
			}
		}
	}
}
