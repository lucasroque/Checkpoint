using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class RecessDAO
    {
        CompanyControl companyControl = new CompanyControl();
        DepartmentControl departmentControl = new DepartmentControl();

        public Boolean saveRecess(Recess recess)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO RECESS (ID_COMPANY, ID_DEPARTMENT, DESCRIPTION, RECESS_DATE) VALUES (?,?,?,?)";

            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = recess.company != null ? recess.company.idCompany : 0;
            cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = recess.department != null ? recess.department.idDepartment : 0;
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = recess.description;
            cmd.Parameters.Add("RECESS_DATE", OleDbType.Date).Value = recess.recessDate;

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

        public Boolean updateRecess(Recess recess)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE RECESS SET ID_COMPANY=?, ID_DEPARTMENT=?,  DESCRIPTION=?, RECESS_DATE=? WHERE ID_RECESS=?";

            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = recess.company != null ? recess.company.idCompany : 0;
            cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = recess.department != null ? recess.department.idDepartment : 0;
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = recess.description;
            cmd.Parameters.Add("RECESS_DATE", OleDbType.Date).Value = recess.recessDate;
            cmd.Parameters.Add("ID_RECESS", OleDbType.Integer).Value = recess.idRecess;

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

        public Boolean deleteRecess(Recess recess)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM RECESS WHERE ID_RECESS=?";
            cmd.Parameters.Add("ID_RECESS", OleDbType.Integer).Value = recess.idRecess;

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

        public List<Recess> getAllRecess()
        {
            List<Recess> recesss = new List<Recess>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM RECESS";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Recess recess = new Recess();
                    recess.idRecess = result.GetInt32(0);
                    recess.company = companyControl.getCompany(result.GetInt32(1));
                    recess.description = result.GetString(2);
                    recess.recessDate = result.GetDateTime(3);
                    recess.department = departmentControl.getDepartment(result.GetInt32(4));

                    recesss.Add(recess);
                }
            }

            result.Close();

            return recesss;
        }

        public List<DateTime> getAllRecessDates()
        {
            List<DateTime> recess = new List<DateTime>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM RECESS";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    recess.Add(result.GetDateTime(3));
                }
            }

            result.Close();

            return recess;
        }

        public Boolean validateRecess(Company company, DateTime date)
        {
            bool valid = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM RECESS WHERE ID_COMPANY=? AND RECESS_DATE=?";
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = company.idCompany;
            cmd.Parameters.Add("RECESS_DATE", OleDbType.Date).Value = date;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                valid = false;
            }

            return valid;
        }
    }
}
