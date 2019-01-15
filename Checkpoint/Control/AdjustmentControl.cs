using Checkpoint.DAO;
using Checkpoint.Model;
using System;

namespace Checkpoint.Control
{
    class AdjustmentControl
    {

        AdjustmentDAO adjustmentDAO = new AdjustmentDAO();

        public Boolean saveAdjustment(Adjustment adjustment)
        {
            return adjustmentDAO.saveAdjustment(adjustment);
        }

        public Boolean updateAdjustment(Adjustment adjustment)
        {
            return adjustmentDAO.updateAdjustment(adjustment);
        }

        public Boolean updateLastMarkingNsr(Int64 lastNsr)
        {
            return adjustmentDAO.updateLastMarkingNsr(lastNsr);
        }

        public Adjustment getAdjustment()
        {
            return adjustmentDAO.getAdjustment();
        }

        public int getGetLastNsr()
        {
            return adjustmentDAO.getGetLastNsr();
        }
    }
}
