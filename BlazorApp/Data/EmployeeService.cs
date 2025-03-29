using FileHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NHapi.Base.Parser;
using NHapi.Model.V251.Message;
using NHapi.Base.Model;
using NHapi.Base.Util;
using NHapi.Model.V251.Group;
using System.Text.RegularExpressions;

namespace BlazorApp.Data
{
	public class EmployeeService
	{
		public List<Employee> employeeList { get; private set; } = new List<Employee>();
		public Employee selectedEmployee { get; set; }
		public bool isLoading { get; private set; } = true;
		public static Dictionary<string, string> locationNames = [];

		// Persistent search state
		public string searchId { get; set; } = string.Empty;
		public string searchName { get; set; } = string.Empty;
		public List<Employee> searchResults { get; set; } = [];

		public event Action? OnEmployeesLoaded;

		public List<IMessage> hl7Messages { get; private set; } = new List<IMessage>();

		public async Task LoadEmployeesAsync()
		{
			// Load employees asynchronously
			if (employeeList.Count == 0)
			{
				employeeList = await Task.Run(() => GetEmployees());
				selectedEmployee = employeeList.First(e => e.ID == "792BDML");
			}
			isLoading = false;
			OnEmployeesLoaded?.Invoke();
		}

		//public async Task LoadHL7RecordsAsync()
		//{
		//	string filePath = Path.Combine("Data", "source_hl7_messages_v2.hl7");
		//	if (!File.Exists(filePath))
		//	{
		//		Console.WriteLine("HL7 file not found.");
		//		return;
		//	}

		//	string hl7Content = await File.ReadAllTextAsync(filePath);
		//	ParseHL7Messages(hl7Content);
		//}


		//private void ParseHL7Messages(string hl7Content)
		//{
		//	// Create the parser instance from NHapi
		//	PipeParser parser = new PipeParser();

		//	// Split the content into individual messages assuming each message starts with "MSH|"
		//	var messages = Regex.Split(hl7Content, @"(?=MSH\|)");

		//	Console.WriteLine(messages.Count());

		//	int i = 0;

		//	foreach (var message in messages)
		//	{
		//		Console.WriteLine(i++);
		//		if (string.IsNullOrWhiteSpace(message))
		//			continue;

		//		try
		//		{
		//			// Parse the HL7 message
		//			IMessage hl7Message = parser.Parse(message);

		//			// Output some information about the parsed message
		//			Console.WriteLine("Parsed message type: " + hl7Message.GetType().Name);

		//			// TODO: Add further processing logic for the parsed message as required.
		//		}
		//		catch (Exception ex)
		//		{
		//			Console.WriteLine("Error parsing message: " + ex.Message);
		//		}
		//	}
		//}



		public async Task LoadHL7RecordsAsync()
		{
			string filePath = Path.Combine("Data", "source_hl7_messages_v2.hl7");
			if (!File.Exists(filePath))
			{
				Console.WriteLine("HL7 file not found.");
				return;
			}

			string hl7Content = await File.ReadAllTextAsync(filePath);
			ParseHL7Messages(hl7Content);
		}

		private void ParseHL7Messages(string hl7Content)
		{
			PipeParser parser = new PipeParser();

			// Split the content into individual messages assuming each message starts with "MSH|"
			var messages = Regex.Split(hl7Content, @"(?=MSH\|)");

			foreach (var message in messages)
			{
				if (string.IsNullOrWhiteSpace(message))
					continue;

				string processedMessage = PreprocessMessage(message);

				try
				{
					IMessage hl7Message = parser.Parse(processedMessage);
					hl7Messages.Add(hl7Message);
					Console.WriteLine("Parsed message type: " + hl7Message.GetType().Name);
					// Further processing logic goes here.
				}
				catch (Exception ex)
				{
					Console.WriteLine("Error parsing message: " + ex.Message);
				}
			}
		}

		/// <summary>
		/// Pre-processes an HL7 message to correct common issues.
		/// - For MSH segments: Ensures MSH-2 (encoding characters) has exactly 4 unique characters.
		/// - For OBX segments: Validates OBX-2 (Value Type) against an allowed set. If invalid (or missing) while OBX-5 is provided,
		///   it is replaced with a default value ("ST").
		/// </summary>
		/// <param name="message">The raw HL7 message</param>
		/// <returns>A processed HL7 message string</returns>
		private string PreprocessMessage(string message)
		{
			// Define allowed OBX-2 values for HL7 version 2.1.
			var allowedTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
		{
			"ST", "NM", "TX", "FT", "CE", "DT", "TM", "TS", "ID", "SI"
		};

			// Split message into individual lines (supporting CR, LF, or CRLF)
			var lines = message.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
			for (int i = 0; i < lines.Length; i++)
			{
				if (lines[i].StartsWith("MSH|"))
				{
					var fields = lines[i].Split('|').ToList();
					if (fields.Count > 1)
					{
						// Check MSH-2 for exactly 4 unique characters.
						string encodingChars = fields[1];
						if (encodingChars.Length != 4 || encodingChars.Distinct().Count() != 4)
						{
							//Console.WriteLine("Correcting MSH-2 encoding characters: " + encodingChars);
							fields[1] = "^~\\&";
						}
						lines[i] = string.Join("|", fields);
					}
				}
				else if (lines[i].StartsWith("OBX|"))
				{
					var fields = lines[i].Split('|').ToList();
					// Ensure the OBX segment has at least 6 fields (OBX-1 to OBX-5)
					if (fields.Count >= 6)
					{
						// If OBX-5 has a value, ensure OBX-2 is valid.
						if (!string.IsNullOrWhiteSpace(fields[5]))
						{
							if (string.IsNullOrWhiteSpace(fields[2]) || !allowedTypes.Contains(fields[2].Trim()))
							{
								//Console.WriteLine($"Fixing OBX segment Value Type. Found: '{fields[2]}', replacing with default 'ST'.");
								fields[2] = "ST";
							}
						}
						lines[i] = string.Join("|", fields);
					}
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
			CSVEmployee?[] records = engine.ReadFile(Path.Combine("Data", "orgchart_faux.csv"));


			List<Employee> employees = [];
			foreach (CSVEmployee? record in records)
			{
				if (record != null)
				{
					Employee employee = new Employee
					{
						ID = record.Emp34Id,
						Name = $"{record.EmpFirstName} {record.EmpLastName}",
						Email = record.EmpEmailAddress,
						Position = record.EmpPositionDesc,
						Location = record.EmpLocationCode,
						Anniversary = string.IsNullOrEmpty(record.EmpAnnivDate) ? "Null" : record.EmpAnnivDate, // Accomodate the CEO who has no anniversary
						Up = null,
						Downs = []
					};
					employees.Add(employee);
					// Add location to locationNames
					if (!locationNames.ContainsKey(record.EmpLocationCode))
					{
						locationNames.Add(record.EmpLocationCode, record.EmpLocationDesc);
					}
				}
			}

			Dictionary<string, Employee> employeeDict = employees.ToDictionary(e => e.ID);

			foreach (CSVEmployee? record in records)
			{
				if (record != null && employeeDict.TryGetValue(record.Emp34Id, out var employee))
				{
					// Set Up reference if manager ID exists
					if (!string.IsNullOrEmpty(record.Mgr34Id) && employeeDict.TryGetValue(record.Mgr34Id, out var manager))
					{
						employee.Up = manager;
						manager.Downs?.Add(employee);
					}
				}
			}
			return employees;
		}
	}
}
