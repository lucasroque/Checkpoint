using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class JustificationControl
    {
        JustificationDAO justificationDAO = new JustificationDAO();

        public Boolean saveJustification(Justification justification)
        {
            return justificationDAO.saveJustification(justification);
        }

        public Boolean updateJustification(Justification justification)
        {
            return justificationDAO.updateJustification(justification);
        }

        public Boolean deleteJustification(Justification justification)
        {
            return justificationDAO.deleteJustification(justification);
        }

        public List<Justification> getAllJustifications()
        {
            return justificationDAO.getAllJustifications();
        }

        public Justification getJustification(int idJustification)
        {
            return justificationDAO.getJustification(idJustification);
        }

        public Boolean validateDescription(String description)
        {
            return justificationDAO.validateDescription(description);
        }
    }
}
