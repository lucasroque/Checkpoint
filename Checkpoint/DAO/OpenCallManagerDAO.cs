using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class OpenCallManagerDAO
    {

        public Boolean saveOpenCallManager(OpenCallManager openCall)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO OPEN_CALL_MANAGER (OCM_USER, OCM_PASSWORD, OCM_HOST, OCM_PORT) VALUES (?,?,?,?)";

            cmd.Parameters.Add("OCM_USER", OleDbType.VarChar).Value = openCall.user;
            cmd.Parameters.Add("OCM_PASSWORD", OleDbType.VarChar).Value = openCall.password;
            cmd.Parameters.Add("OCM_HOST", OleDbType.VarChar).Value = openCall.host;
            cmd.Parameters.Add("OCM_PORT", OleDbType.Integer).Value = openCall.port;

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

        public Boolean updateOpenCallManager(OpenCallManager openCall)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE OPEN_CALL_MANAGER SET OCM_USER=?, OCM_PASSWORD=?, OCM_HOST=?, OCM_PORT=?, OCM_LAST_MARKING_NSR=? WHERE ID_OCM=?";

            cmd.Parameters.Add("OCM_USER", OleDbType.VarChar).Value = openCall.user;
            cmd.Parameters.Add("OCM_PASSWORD", OleDbType.VarChar).Value = openCall.password;
            cmd.Parameters.Add("OCM_HOST", OleDbType.VarChar).Value = openCall.host;
            cmd.Parameters.Add("OCM_PORT", OleDbType.Integer).Value = openCall.port;
            cmd.Parameters.Add("ID_OCM", OleDbType.Integer).Value = openCall.idOpenCallManager;

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
            cmd.CommandText = "SELECT * FROM OPEN_CALL_MANAGER";
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
            cmd.CommandText = "SELECT * FROM OPEN_CALL_MANAGER";
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
            cmd.CommandText = "SELECT * FROM OPEN_CALL_MANAGER";
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
            cmd.CommandText = "SELECT * FROM OPEN_CALL_MANAGER";
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

        public OpenCallManager getOpenCallManager()
        {
            OpenCallManager openCall = null;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM OPEN_CALL_MANAGER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    openCall = new OpenCallManager();
                    openCall.idOpenCallManager = result.GetInt32(0);
                    openCall.user = result.GetString(1);
                    openCall.password = result.GetString(2);
                    openCall.host = result.GetString(3);
                    openCall.port = result.GetInt32(4);
                }
            }

            result.Close();

            return openCall;
        }

    }
}
