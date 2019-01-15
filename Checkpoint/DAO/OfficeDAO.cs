using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class OfficeDAO
    {
        CompanyControl companyControl = new CompanyControl();

        public Boolean saveOffice(Office office)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO OFFICE (DESCRIPTION) VALUES (?)";
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = office.description;

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

        public Boolean updateOffice(Office office)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE OFFICE SET DESCRIPTION=? WHERE ID_OFFICE=?";
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = office.description;
            cmd.Parameters.Add("ID_OFFICE", OleDbType.Integer).Value = office.idOffice;

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

        public Boolean deleteOffice(Office office)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM OFFICE WHERE ID_OFFICE=?";
            cmd.Parameters.Add("ID_OFFICE", OleDbType.Integer).Value = office.idOffice;

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

        public List<Office> getAllOffices()
        {
            List<Office> offices = new List<Office>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM OFFICE";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Office office = new Office();
                    office.idOffice = result.GetInt32(0);
                    office.description = result.GetString(1);

                    offices.Add(office);
                }
            }

            result.Close();

            return offices;
        }

        public Office getOffice(int idOffice)
        {
            Office office = new Office();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM OFFICE WHERE ID_OFFICE=?";
            cmd.Parameters.Add("ID_OFFICE", OleDbType.Integer).Value = idOffice;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    office.idOffice = result.GetInt32(0);
                    office.description = result.GetString(1);
                }
            }

            result.Close();

            return office;
        }

        public Boolean validateDescription(String description)
        {
            bool valid = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM OFFICE WHERE DESCRIPTION=?";
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
