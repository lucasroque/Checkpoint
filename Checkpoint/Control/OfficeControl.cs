using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class OfficeControl
    {
        OfficeDAO officeDAO = new OfficeDAO();

        public Boolean saveOffice(Office office)
        {
            return officeDAO.saveOffice(office);
        }

        public Boolean updateOffice(Office office)
        {
            return officeDAO.updateOffice(office);
        }

        public Boolean deleteOffice(Office office)
        {
            return officeDAO.deleteOffice(office);
        }

        public List<Office> getAllOffices()
        {
            return officeDAO.getAllOffices();
        }

        public Office getOffice(int idOffice)
        {
            return officeDAO.getOffice(idOffice);
        }

        public Boolean validateDescription(String description)
        {
            return officeDAO.validateDescription(description);
        }

    }
}
