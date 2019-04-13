using System;
using AutoMapper;
using MyApp.Core.Commands.Contracts;
using MyApp.Core.ViewModels;
using MyApp.Data;
using MyApp.Models;

namespace MyApp.Core.Commands
{
    public class SetAddressCommand : ICommand
    {
        private readonly MyAppContext context;
        private readonly Mapper mapper;

        public SetAddressCommand(MyAppContext context, Mapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public string Execute(string[] commandParams)
        {
            int employeeId = int.Parse(commandParams[0]);
            string address = commandParams[1];

            Employee employee = this.context.Employees.Find(employeeId);

            if (employee == null)
            {
                throw new InvalidOperationException("User with the given ID doesn't exist!");
            }

            employee.Address = address;
            context.SaveChanges();

            var dto = this.mapper.CreateMappedObject<EmployeeWithAddressDto>(employee);

            string result = $"{dto.FirstName} {dto.LastName} lives at {dto.Address}";

            return result;
        }
    }
}
