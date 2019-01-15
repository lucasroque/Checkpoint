using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class UserProfileDAO
    {

        public Boolean saveUserProfile(UserProfile userProfile)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO USER_PROFILE (DESCRIPTION, SECURITY_LEVEL) VALUES (?,?)";

            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = userProfile.description;
            cmd.Parameters.Add("SECURITY_LEVEL", OleDbType.VarChar).Value = userProfile.securityLevel;

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

        public Boolean updateUserProfile(UserProfile userProfile)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE USER_PROFILE SET DESCRIPTION=?, SECURITY_LEVEL=? WHERE ID_USER_PROFILE=?";

            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = userProfile.description;
            cmd.Parameters.Add("SECURITY_LEVEL", OleDbType.VarChar).Value = userProfile.securityLevel;
            cmd.Parameters.Add("ID_USER_PROFILE", OleDbType.Integer).Value =userProfile.idUserProfile;

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

        public Boolean deleteUserProfile(UserProfile userProfile)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM USER_PROFILE WHERE ID_USER_PROFILE=?";
            cmd.Parameters.Add("ID_USER_PROFILE", OleDbType.Integer).Value = userProfile.idUserProfile;

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

        public List<UserProfile> getAllUserProfile()
        {
            List<UserProfile> userProfiles = new List<UserProfile>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM USER_PROFILE";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    UserProfile userProfile = new UserProfile();
                    userProfile.idUserProfile = result.GetInt32(0);
                    userProfile.description = result.GetString(1);
                    userProfile.securityLevel = result.GetInt32(2);

                    userProfiles.Add(userProfile);
                }
            }

            result.Close();

            return userProfiles;
        }

        public List<UserProfile> getAllUserProfileAvailables()
        {
            List<UserProfile> userProfiles = new List<UserProfile>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM USER_PROFILE WHERE SECURITY_LEVEL > 0";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    UserProfile userProfile = new UserProfile();
                    userProfile.idUserProfile = result.GetInt32(0);
                    userProfile.description = result.GetString(1);
                    userProfile.securityLevel = result.GetInt32(2);

                    userProfiles.Add(userProfile);
                }
            }

            result.Close();

            return userProfiles;
        }

        public UserProfile getUserProfile(Int32 idUserProfile)
        {
            UserProfile userProfile = new UserProfile();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM USER_PROFILE WHERE ID_USER_PROFILE=?";
            cmd.Parameters.Add("ID_USER_PROFILE", OleDbType.Integer).Value = idUserProfile;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    userProfile.idUserProfile = result.GetInt32(0);
                    userProfile.description = result.GetString(1);
                    userProfile.securityLevel = result.GetInt32(2);
                }
            }

            result.Close();

            return userProfile;
        }
    }
}
