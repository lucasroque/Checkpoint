using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class UsersControl
    {
        UserDAO userDAO = new UserDAO();

        public Boolean saveUser(User user)
        {
            return userDAO.saveUser(user);
        }

        public Boolean updateUser(User user)
        {
            return userDAO.updateUser(user);
        }

        public Boolean deleteUser(User user)
        {
            return userDAO.deleteUser(user);
        }

        public List<User> getAllUser()
        {
            return userDAO.getAllUser();
        }

        public User getUser(Int32 idUser)
        {
            return userDAO.getUser(idUser);
        }

        public Boolean validateDescription(String login)
        {
            return userDAO.validateDescription(login);
        }
    }
}
