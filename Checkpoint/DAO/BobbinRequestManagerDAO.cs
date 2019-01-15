using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class BobbinRequestManagerDAO
    {

        public Boolean saveBobbinRequestManager(BobbinRequestManager bobbinRequest)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO BOBBIN_REQUEST_MANAGER (BRM_USER, BRM_PASSWORD, BRM_HOST, BRM_PORT) VALUES (?,?,?,?)";

            cmd.Parameters.Add("BRM_USER", OleDbType.VarChar).Value = bobbinRequest.user;
            cmd.Parameters.Add("BRM_PASSWORD", OleDbType.VarChar).Value = bobbinRequest.password;
            cmd.Parameters.Add("BRM_HOST", OleDbType.VarChar).Value = bobbinRequest.host;
            cmd.Parameters.Add("BRM_PORT", OleDbType.Integer).Value = bobbinRequest.port;

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

        public Boolean updateBobbinRequestManager(BobbinRequestManager bobbinRequest)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE BOBBIN_REQUEST_MANAGER SET BRM_USER=?, BRM_PASSWORD=?, BRM_HOST=?, BRM_PORT=?, BRM_LAST_MARKING_NSR=? WHERE ID_BRM=?";

            cmd.Parameters.Add("BRM_USER", OleDbType.VarChar).Value = bobbinRequest.user;
            cmd.Parameters.Add("BRM_PASSWORD", OleDbType.VarChar).Value = bobbinRequest.password;
            cmd.Parameters.Add("BRM_HOST", OleDbType.VarChar).Value = bobbinRequest.host;
            cmd.Parameters.Add("BRM_PORT", OleDbType.Integer).Value = bobbinRequest.port;
            cmd.Parameters.Add("ID_BRM", OleDbType.Integer).Value = bobbinRequest.idBobbinRequest;

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

        public String getGetUser()
        {
            String callUser = "";

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM BOBBIN_REQUEST_MANAGER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    callUser = result.GetString(1);
                }
            }

            result.Close();

            return callUser;
        }

        public String getGetPassword()
        {
            String callPassword = "";

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM BOBBIN_REQUEST_MANAGER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    callPassword = result.GetString(2);
                }
            }

            result.Close();

            return callPassword;
        }

        public String getGetHost()
        {
            String host = "";

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM BOBBIN_REQUEST_MANAGER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    host = Convert.ToString(result[3]);
                }
            }

            result.Close();

            return host;
        }

        public Int32 getGetPort()
        {
            Int32 port = 0;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM BOBBIN_REQUEST_MANAGER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    port = Convert.ToInt32(result[4]);
                }
            }

            result.Close();

            return port;
        }

        public BobbinRequestManager getBobbinRequestManager()
        {
            BobbinRequestManager bobbinRequest = null;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM BOBBIN_REQUEST_MANAGER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    bobbinRequest = new BobbinRequestManager();
                    bobbinRequest.idBobbinRequest = result.GetInt32(0);
                    bobbinRequest.user = result.GetString(1);
                    bobbinRequest.password = result.GetString(2);
                    bobbinRequest.host = result.GetString(3);
                    bobbinRequest.port = result.GetInt32(4);
                }
            }

            result.Close();

            return bobbinRequest;
        }

    }
}
