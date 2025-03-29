using FileHelpers;

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
            CSVEmployee?[] records = engine.ReadFile("Data\\orgchart_faux.csv");

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
