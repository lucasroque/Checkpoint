using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class JustificationDAO
    {
        CompanyControl companyControl = new CompanyControl();

        public Boolean saveJustification(Justification justification)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO JUSTIFICATION (DESCRIPTION) VALUES (?)";
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = justification.description;

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

        public Boolean updateJustification(Justification justification)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE JUSTIFICATION SET DESCRIPTION=? WHERE ID_JUSTIFICATION=?";

            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = justification.description;
            cmd.Parameters.Add("ID_JUSTIFICATION", OleDbType.Integer).Value = justification.idJustification;

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

        public Boolean deleteJustification(Justification justification)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM JUSTIFICATION WHERE ID_JUSTIFICATION=?";
            cmd.Parameters.Add("ID_JUSTIFICATION", OleDbType.Integer).Value = justification.idJustification;

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

        public List<Justification> getAllJustifications()
        {
            List<Justification> justifications = new List<Justification>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM JUSTIFICATION";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Justification justification = new Justification();
                    justification.idJustification = result.GetInt32(0);
                    justification.description = result.GetString(1);

                    justifications.Add(justification);
                }
            }

            result.Close();

            return justifications;
        }

        public Justification getJustification(int idJustification)
        {
            Justification justification = new Justification();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM JUSTIFICATION WHERE ID_JUSTIFICATION=?";
            cmd.Parameters.Add("ID_JUSTIFICATION", OleDbType.Integer).Value = idJustification;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    justification.idJustification = result.GetInt32(0);
                    justification.description = result.GetString(1);
                }
            }

            result.Close();

            return justification;
        }

        public Boolean validateDescription(String description)
        {
            bool valid = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM JUSTIFICATION WHERE DESCRIPTION=?";
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
