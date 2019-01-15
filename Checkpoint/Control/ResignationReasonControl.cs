using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class ResignationReasonControl
    {
        ResignationReasonDAO resignationReasonDAO = new ResignationReasonDAO();

        public Boolean saveResignationReason(ResignationReason resignationReason)
        {
            return resignationReasonDAO.saveResignationReason(resignationReason);
        }

        public Boolean updateResignationReason(ResignationReason resignationReason)
        {
            return resignationReasonDAO.updateResignationReason(resignationReason);
        }

        public Boolean deleteResignationReason(ResignationReason resignationReason)
        {
            return resignationReasonDAO.deleteResignationReason(resignationReason);
        }

        public List<ResignationReason> getAllResignationReasons()
        {
            return resignationReasonDAO.getAllResignationReasons();
        }

        public ResignationReason getResignationReason(int idResignationReason)
        {
            return resignationReasonDAO.getResignationReason(idResignationReason);
        }

    }
}
