using Checkpoint.DAO;
using Checkpoint.Model;
using System;

namespace Checkpoint.Control
{
    class BobbinRequestManagerControl
    {

        BobbinRequestManagerDAO bobbinRequestDAO = new BobbinRequestManagerDAO();

        public Boolean saveBobbinRequestManager(BobbinRequestManager bobbinRequest)
        {
            return bobbinRequestDAO.saveBobbinRequestManager(bobbinRequest);
        }

        public Boolean updateBobbinRequestManager(BobbinRequestManager bobbinRequest)
        {
            return bobbinRequestDAO.updateBobbinRequestManager(bobbinRequest);
        }

        public BobbinRequestManager getBobbinRequestManager()
        {
            return bobbinRequestDAO.getBobbinRequestManager();
        }

        public String getGetUser()
        {
            return bobbinRequestDAO.getGetUser();
        }

        public String getGetPassword()
        {
            return bobbinRequestDAO.getGetPassword();
        }

        public String getGetHost()
        {
            return bobbinRequestDAO.getGetHost();
        }

        public Int32 getGetPort()
        {
            return bobbinRequestDAO.getGetPort();
        }
    }
}
