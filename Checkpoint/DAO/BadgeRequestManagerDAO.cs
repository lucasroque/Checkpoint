using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class BadgeRequestManagerDAO
    {

        public Boolean saveBadgeRequestManager(BadgeRequestManager badgeRequest)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO BADGE_REQUEST_MANAGER (BGRM_USER, BGRM_PASSWORD, BGRM_HOST, BGRM_PORT) VALUES (?,?,?,?)";

            cmd.Parameters.Add("BGRM_USER", OleDbType.VarChar).Value = badgeRequest.user;
            cmd.Parameters.Add("BGRM_PASSWORD", OleDbType.VarChar).Value = badgeRequest.password;
            cmd.Parameters.Add("BGRM_HOST", OleDbType.VarChar).Value = badgeRequest.host;
            cmd.Parameters.Add("BGRM_PORT", OleDbType.Integer).Value = badgeRequest.port;

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

        public Boolean updateBadgeRequestManager(BadgeRequestManager badgeRequest)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE BADGE_REQUEST_MANAGER SET BGRM_USER=?, BGRM_PASSWORD=?, BGRM_HOST=?, BGRM_PORT=?, BGRM_LAST_MARKING_NSR=? WHERE ID_BGRM=?";

            cmd.Parameters.Add("BGRM_USER", OleDbType.VarChar).Value = badgeRequest.user;
            cmd.Parameters.Add("BGRM_PASSWORD", OleDbType.VarChar).Value = badgeRequest.password;
            cmd.Parameters.Add("BGRM_HOST", OleDbType.VarChar).Value = badgeRequest.host;
            cmd.Parameters.Add("BGRM_PORT", OleDbType.Integer).Value = badgeRequest.port;
            cmd.Parameters.Add("ID_BGRM", OleDbType.Integer).Value = badgeRequest.idBadgeRequest;

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
            cmd.CommandText = "SELECT * FROM BADGE_REQUEST_MANAGER";
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
            cmd.CommandText = "SELECT * FROM BADGE_REQUEST_MANAGER";
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
            cmd.CommandText = "SELECT * FROM BADGE_REQUEST_MANAGER";
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
            cmd.CommandText = "SELECT * FROM BADGE_REQUEST_MANAGER";
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

        public BadgeRequestManager getBadgeRequestManager()
        {
            BadgeRequestManager badgeRequest = null;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM BADGE_REQUEST_MANAGER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    badgeRequest = new BadgeRequestManager();
                    badgeRequest.idBadgeRequest = result.GetInt32(0);
                    badgeRequest.user = result.GetString(1);
                    badgeRequest.password = result.GetString(2);
                    badgeRequest.host = result.GetString(3);
                    badgeRequest.port = result.GetInt32(4);
                }
            }

            result.Close();

            return badgeRequest;
        }

    }
}
