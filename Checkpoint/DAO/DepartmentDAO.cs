using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;


namespace Checkpoint.DAO
{
    class DepartmentDAO
    {
        CompanyControl companyControl = new CompanyControl();

        public Boolean saveDepartment(Department department)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO DEPARTMENT (DESCRIPTION) VALUES (?)";
            
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = department.description;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao salvar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean updateDepartment(Department department)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE DEPARTMENT SET DESCRIPTION=? WHERE ID_DEPARTMENT=?";
            
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = department.description;
            cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = department.idDepartment;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean deleteDepartment(Department department)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM DEPARTMENT WHERE ID_DEPARTMENT=?";
            cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = department.idDepartment;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao excluir!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public List<Department> getAllDepartments()
        {
            List<Department> departments = new List<Department>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM DEPARTMENT";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Department department = new Department();
                    department.idDepartment = result.GetInt32(0);
                    department.description = result.GetString(1);

                    departments.Add(department);
                }
            }

            result.Close();

            return departments;
        }

        public Department getDepartment(int idDepartment)
        {
            Department department = new Department();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM DEPARTMENT WHERE ID_DEPARTMENT=?";
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = idDepartment;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    department.idDepartment = result.GetInt32(0);
                    department.description = result.GetString(1);
                }
            }

            result.Close();

            return department;
        }

        public Boolean validateDescription(String description)
        {
            bool valid = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM DEPARTMENT WHERE DESCRIPTION=?";
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = description;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                valid = false;
            }

            return valid;
        }
    }
}
