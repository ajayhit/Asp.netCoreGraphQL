using Asp.netCoreGraphQL.Model;
using Asp.netCoreGraphQL.Repository;
using HotChocolate;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Asp.netCoreGraphQL.DataAccess
{
    public class Query
    {
        public List<Employee> AllEmployeeOnly([Service] EmployeeRepository employeeRepository) =>
          employeeRepository.GetEmployees();

        public List<Employee> AllEmployeeWithDepartment([Service] EmployeeRepository employeeRepository) =>
            employeeRepository.GetEmployeesWithDepartment();

        public async Task<Employee> GetEmployeeById([Service] EmployeeRepository employeeRepository,
            [Service] ITopicEventSender eventSender, int id)
        {
            Employee gottenEmployee = employeeRepository.GetEmployeeById(id);
            await eventSender.SendAsync("ReturnedEmployee", gottenEmployee);
            return gottenEmployee;
        }
        public async Task<Employee> UpdateEmployeeById([Service] EmployeeRepository employeeRepository,
           [Service] ITopicEventSender eventSender, int id, string name)
        {
            Employee gottenEmployee = await employeeRepository.UPdateEmployeeByIdAsync(id, name);
            await eventSender.SendAsync("ReturnedEmployee", gottenEmployee);
            return gottenEmployee;
        }


        public List<Department> AllDepartmentsOnly([Service] DepartmentRepository departmentRepository) =>
            departmentRepository.GetAllDepartmentOnly();

        public List<Department> AllDepartmentsWithEmployee([Service] DepartmentRepository departmentRepository) =>
            departmentRepository.GetAllDepartmentsWithEmployee();

    }
}
