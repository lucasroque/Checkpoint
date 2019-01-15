using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class UserProfilesControl
    {
        UserProfileDAO userProfileDAO = new UserProfileDAO();

        public Boolean saveUserProfile(UserProfile userProfile)
        {
            return userProfileDAO.saveUserProfile(userProfile);
        }

        public Boolean updateUserProfile(UserProfile userProfile)
        {
            return userProfileDAO.updateUserProfile(userProfile);
        }

        public Boolean deleteUserProfile(UserProfile userProfile)
        {
            return userProfileDAO.deleteUserProfile(userProfile);
        }

        public List<UserProfile> getAllUserProfile()
        {
            return userProfileDAO.getAllUserProfile();
        }

        public List<UserProfile> getAllUserProfileAvailables()
        {
            return userProfileDAO.getAllUserProfileAvailables();
        }

        public UserProfile getUserProfile(Int32 idUserProfile)
        {
            return userProfileDAO.getUserProfile(idUserProfile);
        }

        public List<Int32> getSecurityLevels()
        {
            List<Int32> securityLevels = new List<Int32>();

            securityLevels.Add(0);
            securityLevels.Add(1);
            securityLevels.Add(2);
            securityLevels.Add(3);
            securityLevels.Add(4);
            securityLevels.Add(5);

            return securityLevels;
        }

    }
}
