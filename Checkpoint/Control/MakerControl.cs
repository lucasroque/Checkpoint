using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class MakerControl
    {

        MakerDAO makerDAO = new MakerDAO();

        public List<Maker> getAllMakers()
        {
            return makerDAO.getAllMakers();
        }

        public Maker getMaker(Int32 idMaker)
        {
            return makerDAO.getMaker(idMaker);
        }
    }
}
