using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class SetManagerCommand : ICommand
    {
        private readonly MyAppContext context;

        public SetManagerCommand(MyAppContext context)
        {
            this.context = context;
        }

        public string Execute(string[] commandParams)
        {
            int employeeId = int.Parse(commandParams[0]);
            int managerId = int.Parse(commandParams[1]);

            Employee employee = this.context.Employees.Find(employeeId);
            Employee manager = this.context.Employees.Find(managerId);

            if (employee == null || manager == null)
            {
                throw new InvalidOperationException("Employee or manager with the given ID doesn't exist in the database!");
            }

            employee.Manager = manager;;
            this.context.SaveChanges();

            string result =
                $"{manager.FirstName} {manager.LastName} is manager to {employee.FirstName} {employee.LastName}";

            return result;
        }
    }
}
