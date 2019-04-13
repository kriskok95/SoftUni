using System;
using System.Text;
using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class EmployeePersonalInfoCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public EmployeePersonalInfoCommand(MyAppContext context, Mapper mapper)
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
                throw new InvalidOperationException("The username with the given ID doesn't exist!");
            }

            StringBuilder sb = new StringBuilder();

            var personalInfoDto = this.mapper.CreateMappedObject<EmployeeInfoDto>(employee);

            sb.AppendLine(
                $"ID: {personalInfoDto.Id} - {personalInfoDto.FirstName} {personalInfoDto.LastName} - ${personalInfoDto.Salary}");
            sb.AppendLine($"Birthday: {personalInfoDto.Birthday.Value.ToString("dd-MM-yyyy")}");
            sb.Append($"Address: {personalInfoDto.Address}");

            return sb.ToString();
        }
    }
}
