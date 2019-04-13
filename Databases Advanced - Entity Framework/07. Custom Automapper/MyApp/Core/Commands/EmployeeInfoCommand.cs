using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class EmployeeInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public EmployeeInfoCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandParams)
        {
            int employeeId = int.Parse(commandParams[0]);

            Employee employee = this.context.Employees.Find(employeeId);


            if (employee == null)
            {
                throw new InvalidOperationException("User with the given ID doesn't exist!");
            }

            var EmployeeInfoDto = this.mapper.CreateMappedObject<EmployeeInfoDto>(employee);

            var result =
                $"ID: {EmployeeInfoDto.Id} - {EmployeeInfoDto.FirstName} {EmployeeInfoDto.LastName} -  ${EmployeeInfoDto.Salary:F2}";

            return result;
        }
    }
}
