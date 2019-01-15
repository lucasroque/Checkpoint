using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class DepartmentControl
    {
        DepartmentDAO departmentDAO = new DepartmentDAO();

        public Boolean saveDepartment(Department department)
        {
            return departmentDAO.saveDepartment(department);
        }

        public Boolean updateDepartment(Department department)
        {
            return departmentDAO.updateDepartment(department);
        }

        public Boolean deleteDepartment(Department department)
        {
            return departmentDAO.deleteDepartment(department);
        }

        public List<Department> getAllDepartments()
        {
            return departmentDAO.getAllDepartments();
        }

        public Department getDepartment(int idDepartment)
        {
            return departmentDAO.getDepartment(idDepartment);
        }

        public Boolean validateDescription(String description)
        {
            return departmentDAO.validateDescription(description);
        }
    }
}
