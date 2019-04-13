using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class ManagerInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public ManagerInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandParams)
        {
            int managerId = int.Parse(commandParams[0]);

            Employee manager = this.context.Employees
                .Include(m => m.ManagedEmployees)
                .FirstOrDefault(x => x.Id == managerId);

            if (manager == null)
            {
                throw new InvalidProgramException("Employee with the given ID doesn't exist!");
            }


            //TODO: Ask why it doesn't work???
            //var managerDto = this.mapper.CreateMappedObject<ManagerDto>(manager);

            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine(
                $"{manager.FirstName} {manager.LastName} | Employees: {manager.ManagedEmployees.Count}");

            foreach (var managedEmployee in manager.ManagedEmployees)
            {
                sb.AppendLine(
                    $"- {managedEmployee.FirstName} {managedEmployee.LastName} - ${managedEmployee.Salary:F2}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
