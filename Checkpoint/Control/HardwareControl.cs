using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class HardwareControl
    {
        HardwareDAO hardwareDAO = new HardwareDAO();

        public Boolean saveHardware(Hardware hardware)
        {
            return hardwareDAO.saveHardware(hardware);
        }

        public Boolean updateHardware(Hardware hardware)
        {
            return hardwareDAO.updateHardware(hardware);
        }

        public Boolean deleteHardware(Hardware hardware)
        {
            return hardwareDAO.deleteHardware(hardware);
        }

        public List<Hardware> getAllHardwares()
        {
            return hardwareDAO.getAllHardwares();
        }

        public Hardware getHardware(int idHardware)
        {
            return hardwareDAO.getHardware(idHardware);
        }

    }
}
