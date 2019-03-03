using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using SoftUni.Data;
using SoftUni.Models;

namespace SoftUni
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new SoftUniContext())
            {
                var result = RemoveTown(new SoftUniContext());
                Console.WriteLine(result);
            }
        }

        public static string GetEmployeesFullInformation(SoftUniContext context)
        {
            StringBuilder sb = new StringBuilder();

            var filteredEmployees = context.Employees
                .OrderBy(x => x.EmployeeId)
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.MiddleName,
                    x.JobTitle,
                    x.Salary
                });

            foreach (var employee in filteredEmployees)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} {employee.MiddleName} {employee.JobTitle} {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesWithSalaryOver50000(SoftUniContext context)
        {
            var EmployeesWithSalary = context.Employees
                .Where(x => x.Salary > 50000)
                .Select(x => new
                {
                    x.FirstName,
                    x.Salary
                })
                .OrderBy(x => x.FirstName)
                .ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var employee in EmployeesWithSalary)
            {
                sb.AppendLine($"{employee.FirstName} - {employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesFromResearchAndDevelopment(SoftUniContext context)
        {
            var employeesFromDepartments = context.Employees
                .Where(x => x.Department.Name.Equals("Research and Development"))
                .Select(x => new
                {
                    x.FirstName,
                    x.LastName,
                    x.Department.Name,
                    x.Salary
                })
                .OrderBy(x => x.Salary)
                .ThenByDescending(x => x.FirstName)
                .ToList();


            StringBuilder sb = new StringBuilder();

            foreach (var employee in employeesFromDepartments)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} from {employee.Name} - ${employee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }

        public static string AddNewAddressToEmployee(SoftUniContext context)
        {
            Address address = new Address();
            address.TownId = 4;
            address.AddressText = "Vitoshka 15";
            context.Addresses.Add(address);

            Employee empNakov = context.Employees
                .FirstOrDefault(x => x.LastName.Equals("Nakov"));

            empNakov.Address = address;

            context.SaveChanges();

            var employees = context.Employees
                .Select(x => new
                {
                    x.Address.AddressText,
                    x.AddressId
                })
                .OrderByDescending(x => x.AddressId)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine(employee.AddressText);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesInPeriod(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(e => e.EmployeesProjects.Any(x => x.Project.StartDate.Year >= 2001
                                                         && x.Project.StartDate.Year <= 2003))
                .Select(x => new
                {
                    EmployeeFullName = x.FirstName + " " + x.LastName,
                    ManagerFullName = x.Manager.FirstName + " " + x.Manager.LastName,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name,
                        StartDate = p.Project.StartDate,
                        EndDate = p.Project.EndDate
                    })
                })
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.EmployeeFullName} - Manager: {employee.ManagerFullName}");

                foreach (var project in employee.Projects)
                {
                    string startDate = project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);

                    string endDate = project.EndDate.HasValue
                        ? project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
                        : "not finished";

                    sb.AppendLine($"--{project.ProjectName} - {startDate} - {endDate}");
                }
            }

            return sb.ToString().TrimEnd();

        }

        public static string GetAddressesByTown(SoftUniContext context)
        {
            var addresses = context.Addresses
                .Select(a => new
                {
                    AddressText = a.AddressText,
                    TownName = a.Town.Name,
                    Employees = a.Employees.Count
                })
                .OrderByDescending(x => x.Employees)
                .ThenBy(x => x.TownName)
                .ThenBy(x => x.AddressText)
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.Employees} employees");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployee147(SoftUniContext context)
        {
            var emp = context.Employees
                .Where(e => e.EmployeeId.Equals(147))
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                    JobTitle = x.JobTitle,
                    Projects = x.EmployeesProjects.Select(p => new
                    {
                        ProjectName = p.Project.Name
                    })
                })
                .FirstOrDefault();


            var sb = new StringBuilder();

            sb.AppendLine($"{emp.FullName} - {emp.JobTitle}");

            foreach (var project in emp.Projects.OrderBy(x => x.ProjectName))
            {
                sb.AppendLine(project.ProjectName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetDepartmentsWithMoreThan5Employees(SoftUniContext context)
        {
            var departments = context.Departments
                .Where(e => e.Employees.Count > 5)
                .Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerName = d.Manager.FirstName + " " + d.Manager.LastName,
                    Employees = d.Employees.Select(e => new
                    {
                        FirstName = e.FirstName,
                        LastName = e.LastName,
                        JobTitle = e.JobTitle
                    })
                })
                .OrderBy(x => x.Employees.Count())
                .ThenBy(x => x.DepartmentName);

            var sb = new StringBuilder();

            foreach (var department in departments)
            {
                sb.AppendLine($"{department.DepartmentName} - {department.ManagerName}");

                foreach (var employee in department.Employees
                    .OrderBy(n => n.FirstName)
                    .ThenBy(n => n.LastName))
                {
                    sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle}");
                }
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetLatestProjects(SoftUniContext context)
        {
            var projects = context.Projects
                .OrderByDescending(d => d.StartDate)
                .Take(10)
                .Select(p => new
                {
                    ProjectName = p.Name,
                    Description = p.Description,
                    StartDate = p.StartDate
                })
                .OrderBy(x => x.ProjectName)   
          
                .ToList();

            var sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine(project.ProjectName);
                sb.AppendLine(project.Description);
                sb.AppendLine(project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture));
            }

            return sb.ToString().TrimEnd();
        }

        public static string IncreaseSalaries(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(d => d.Department.Name.Equals("Engineering")
                            || d.Department.Name.Equals("Tool Design")
                            || d.Department.Name.Equals("Marketing")
                            || d.Department.Name.Equals("Information Services"))
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Salary = x.Salary + (x.Salary * 0.12M)
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName)
                .ToList();

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniContext context)
        {
            var employees = context.Employees
                .Where(x => x.FirstName.StartsWith("Sa"))
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    JobTitle = e.JobTitle,
                    Salary = e.Salary
                })
                .OrderBy(x => x.FirstName)
                .ThenBy(x => x.LastName);

            var sb = new StringBuilder();

            foreach (var employee in employees)
            {
                sb.AppendLine(
                    $"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - (${employee.Salary:F2})");
            }

            return sb.ToString().TrimEnd();
        }

        public static string DeleteProjectById(SoftUniContext context)
        {
            var projectForDelete = context.Projects.Find(2);

            var employeeProjects = context.EmployeesProjects
                .Where(x => x.ProjectId == 2)
                .ToList();

            context.EmployeesProjects.RemoveRange(employeeProjects);

            context.Projects.Remove(projectForDelete);

            context.SaveChanges();

            var projects = context.Projects
                .Take(10)
                .Select(x => new
                {
                    ProjectName = x.Name
                })
                .ToList();

            var sb = new StringBuilder();

            foreach (var project in projects)
            {
                sb.AppendLine(project.ProjectName);
            }

            return sb.ToString().TrimEnd();
        }

        public static string RemoveTown(SoftUniContext context)
        {
            int addressesCount = context.Addresses
                .Count(x => x.Town.Name.Equals("Seattle"));

            Town town = context.Towns
                .FirstOrDefault(x => x.Name.Equals("Seattle"));

            var addresses = context.Addresses
                .Where(t => t.Town.Name.Equals("Seattle"))
                .ToList();

            context.Employees
                .Where(x => x.Address.Town.Name.Equals("Seattle"))
                .ToList()
                .ForEach(e => e.AddressId = null);
               
            context.Addresses.RemoveRange(addresses);

            context.Towns.Remove(town);

            context.SaveChanges();

            return $"{addressesCount} addresses in Seattle were deleted";
        }
    }
}
