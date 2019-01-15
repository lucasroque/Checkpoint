using Checkpoint.DAO;
using Checkpoint.Model;
using System;

namespace Checkpoint.Control
{
    class OpenCallManagerControl
    {

        OpenCallManagerDAO openCallDAO = new OpenCallManagerDAO();

        public Boolean saveOpenCallManager(OpenCallManager openCall)
        {
            return openCallDAO.saveOpenCallManager(openCall);
        }

        public Boolean updateOpenCallManager(OpenCallManager openCall)
        {
            return openCallDAO.updateOpenCallManager(openCall);
        }

        public OpenCallManager getOpenCallManager()
        {
            return openCallDAO.getOpenCallManager();
        }

        public String getUser()
        {
            return openCallDAO.getGetUser();
        }

        public String getPassword()
        {
            return openCallDAO.getGetPassword();
        }

        public String getHost()
        {
            return openCallDAO.getGetHost();
        }

        public Int32 getPort()
        {
            return openCallDAO.getGetPort();
        }
    }
}
