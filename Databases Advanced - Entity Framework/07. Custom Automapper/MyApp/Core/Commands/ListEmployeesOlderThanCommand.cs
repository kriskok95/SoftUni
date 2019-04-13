using System;
using System.Linq;
using System.Text;
using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;

namespace MyApp.Core.Commands
{
    public class ListEmployeesOlderThanCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ListEmployeesOlderThanCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandParams)
        {
            int age = int.Parse(commandParams[0]);

            var employees = this.context.Employees
                .Where(a => DateTime.Now.Year - a.Birthday.Value.Year > age)
                .OrderByDescending(s => s.Salary)
                .ToArray();

            if (!employees.Any())
            {
                throw new InvalidOperationException("There isn't employees older the the given age!");
            }

            StringBuilder sb = new StringBuilder();

            //TODO: Check why DTO doesn't work???
            foreach (var employee in employees)
            {
                string manager = employee.Manager == null ? "[No manager]" : $"{employee.Manager.FirstName}";

                sb.AppendLine($"{employee.FirstName} {employee.LastName} - ${employee.Salary} - Manager: {manager}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
