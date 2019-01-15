using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class RecessControl
    {
        RecessDAO recessDAO = new RecessDAO();

        public Boolean saveRecess(Recess recess)
        {
            return recessDAO.saveRecess(recess);
        }

        public Boolean updateRecess(Recess recess)
        {
            return recessDAO.updateRecess(recess);
        }

        public Boolean deleteRecess(Recess recess)
        {
            return recessDAO.deleteRecess(recess);
        }

        public List<Recess> getAllRecess()
        {
            return recessDAO.getAllRecess();
        }

        public List<DateTime> getAllRecessDates()
        {
            return recessDAO.getAllRecessDates();
        }

        public Boolean validateRecess(Company company, DateTime date)
        {
            return recessDAO.validateRecess(company, date);
        }
    }
}
