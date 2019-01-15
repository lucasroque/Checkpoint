using Checkpoint.DAO;
using Checkpoint.Model;
using System;

namespace Checkpoint.Control
{
    class BadgeRequestManagerControl
    {

        BadgeRequestManagerDAO badgeRequestDAO = new BadgeRequestManagerDAO();

        public Boolean saveBadgeRequestManager(BadgeRequestManager badgeRequest)
        {
            return badgeRequestDAO.saveBadgeRequestManager(badgeRequest);
        }

        public Boolean updateBadgeRequestManager(BadgeRequestManager badgeRequest)
        {
            return badgeRequestDAO.updateBadgeRequestManager(badgeRequest);
        }

        public BadgeRequestManager getBadgeRequestManager()
        {
            return badgeRequestDAO.getBadgeRequestManager();
        }

        public String getGetUser()
        {
            return badgeRequestDAO.getGetUser();
        }

        public String getGetPassword()
        {
            return badgeRequestDAO.getGetPassword();
        }

        public String getGetHost()
        {
            return badgeRequestDAO.getGetHost();
        }

        public Int32 getGetPort()
        {
            return badgeRequestDAO.getGetPort();
        }
    }
}
