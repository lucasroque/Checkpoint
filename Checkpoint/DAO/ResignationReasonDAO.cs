using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class ResignationReasonDAO
    {
        CompanyControl companyControl = new CompanyControl();

        public Boolean saveResignationReason(ResignationReason resignationReason)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO RESIGNATION_REASON (DESCRIPTION) VALUES (?)";
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = resignationReason.description;

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

        public Boolean updateResignationReason(ResignationReason resignationReason)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE RESIGNATION_REASON SET DESCRIPTION=? WHERE ID_RESIGNATION_REASON=?";

            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = resignationReason.description;
            cmd.Parameters.Add("ID_RESIGNATION_REASON", OleDbType.Integer).Value = resignationReason.idResignationReason;

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

        public Boolean deleteResignationReason(ResignationReason resignationReason)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM RESIGNATION_REASON WHERE ID_RESIGNATION_REASON=?";
            cmd.Parameters.Add("ID_RESIGNATION_REASON", OleDbType.Integer).Value = resignationReason.idResignationReason;

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

        public List<ResignationReason> getAllResignationReasons()
        {
            List<ResignationReason> resignationReasons = new List<ResignationReason>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM RESIGNATION_REASON";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    ResignationReason resignationReason = new ResignationReason();
                    resignationReason.idResignationReason = result.GetInt32(0);
                    resignationReason.description = result.GetString(1);

                    resignationReasons.Add(resignationReason);
                }
            }

            result.Close();

            return resignationReasons;
        }

        public ResignationReason getResignationReason(int idResignationReason)
        {
            ResignationReason resignationReason = new ResignationReason();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM RESIGNATION_REASON WHERE ID_RESIGNATION_REASON=?";
            cmd.Parameters.Add("ID_RESIGNATION_REASON", OleDbType.Integer).Value = idResignationReason;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    resignationReason.idResignationReason = result.GetInt32(0);
                    resignationReason.description = result.GetString(1);
                }
            }

            result.Close();

            return resignationReason;
        }
    }
}
