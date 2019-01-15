using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class EmployeeControl
    {
        EmployeeDAO employeeDAO = new EmployeeDAO();

        public Boolean saveImportEmployee(String name, String pisPasep, int idCompany, int idSchedule, String leefNumber, DateTime admission)
        {
            return employeeDAO.saveImportEmployee(name, pisPasep, idCompany, idSchedule, leefNumber, admission);
        }

        public Boolean saveEmployee(Employee employee)
        {
            return employeeDAO.saveEmployee(employee);
        }

        public Boolean updateEmployee(Employee employee)
        {
            return employeeDAO.updateEmployee(employee);
        }

        public Boolean deleteEmployee(Employee employee)
        {
            return employeeDAO.deleteEmployee(employee);
        }

        public Employee getEmployee(int idEmployee)
        {
            return employeeDAO.getEmployee(idEmployee);
        }

        public Employee getEmployeeByPisPasep(String pisPasep)
        {
            return getEmployeeByPisPasep(pisPasep);
        }

        public List<Employee> getAllEmployees()
        {
            return employeeDAO.getAllEmployees();
        }

        public List<Employee> getAllEmployeesControl()
        {
            List<Employee> allEmployees = employeeDAO.getAllEmployees();
            allEmployees.Insert(0, new Employee(0, "Todos"));
            return allEmployees;
        }

        public List<Employee> getAllEmployeesFromDepartment(int idDepartment, int idOffice)
        {
            return employeeDAO.getAllEmployeesFromDepartment(idDepartment, idOffice);
        }

        public bool validadePisPasep(string pisPasep)
        {
            return employeeDAO.validadePisPasep(pisPasep);
        }

        public bool validadeLeefNumber(string leefNumber)
        {
            return employeeDAO.validadeLeefNumber(leefNumber);
        }
    }
}
