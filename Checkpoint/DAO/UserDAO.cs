using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.DAO
{
    class UserDAO
    {
        UserProfilesControl userProfilesControl = new UserProfilesControl();

        public Boolean saveUser(User user)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO USERS (ID_USER_PROFILE, USER_LOGIN, USER_PASSWORD, USER_VALIDITY, ACTIVE_USER) VALUES (?,?,?,?,?)";

            cmd.Parameters.Add("ID_USER_PROFILE", OleDbType.Integer).Value = user.userProfile != null ? user.userProfile.idUserProfile : 0;
            cmd.Parameters.Add("USER_LOGIN", OleDbType.VarChar).Value = user.login;
            cmd.Parameters.Add("USER_PASSWORD", OleDbType.VarChar).Value = user.password;
            cmd.Parameters.Add("USER_VALIDITY", OleDbType.Date).Value = user.validity;
            cmd.Parameters.Add("ACTIVE_USER", OleDbType.Boolean).Value = user.active;

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

        public Boolean updateUser(User user)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE USERS SET ID_USER_PROFILE=?, USER_LOGIN=?,  USER_PASSWORD=?, USER_VALIDITY=?, ACTIVE_USER=? WHERE ID_USER=?";

            cmd.Parameters.Add("ID_USER_PROFILE", OleDbType.Integer).Value = user.userProfile != null ? user.userProfile.idUserProfile : 0;
            cmd.Parameters.Add("USER_LOGIN", OleDbType.VarChar).Value = user.login;
            cmd.Parameters.Add("USER_PASSWORD", OleDbType.VarChar).Value = user.password;
            cmd.Parameters.Add("USER_VALIDITY", OleDbType.Date).Value = user.validity;
            cmd.Parameters.Add("ACTIVE_USER", OleDbType.Boolean).Value = user.active;
            cmd.Parameters.Add("ID_USER", OleDbType.Integer).Value = user.idUser;

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

        public Boolean deleteUser(User user)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM USERS WHERE ID_USER=?";
            cmd.Parameters.Add("ID_USERS", OleDbType.Integer).Value = user.idUser;

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

        public List<User> getAllUser()
        {
            List<User> users = new List<User>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM USERS";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    User user = new User();
                    user.idUser = result.GetInt32(0);
                    user.userProfile = userProfilesControl.getUserProfile(result.GetInt32(1));
                    user.login = result.GetString(2);
                    user.password = result.GetString(3);
                    user.validity = result.GetDateTime(4);
                    user.active = result.GetBoolean(5);

                    users.Add(user);
                }
            }

            result.Close();

            return users;
        }

        public User getUser(Int32 idUser)
        {
            User user = new User();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM USERS WHERE ID_USER=?";
            cmd.Parameters.Add("ID_USERS", OleDbType.Integer).Value = idUser;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {    
                    user.idUser = result.GetInt32(0);
                    user.userProfile = userProfilesControl.getUserProfile(result.GetInt32(1));
                    user.login = result.GetString(2);
                    user.password = result.GetString(3);
                    user.validity = result.GetDateTime(4);
                    user.active = result.GetBoolean(5);
                }
            }

            result.Close();

            return user;
        }

        public Boolean validateDescription(String login)
        {
            bool valid = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM USERS WHERE USER_LOGIN=?";
            cmd.Parameters.Add("USER_LOGIN", OleDbType.VarChar).Value = login;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                valid = false;
            }

            return valid;
        }
    }
}
