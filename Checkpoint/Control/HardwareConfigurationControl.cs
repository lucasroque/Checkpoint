using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class HardwareConfigurationControl
    {
        HardwareConfigurationDAO hardwareConfigurationDAO = new HardwareConfigurationDAO();

        public Boolean saveHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            return hardwareConfigurationDAO.saveHardwareConfiguration(hardwareConfiguration);
        }

        public Boolean updateHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            return hardwareConfigurationDAO.updateHardwareConfiguration(hardwareConfiguration);
        }

        public Boolean deleteHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            return hardwareConfigurationDAO.deleteHardwareConfiguration(hardwareConfiguration);
        }

        public List<HardwareConfiguration> getAllHardwareConfigurations()
        {
            return hardwareConfigurationDAO.getAllHardwareConfigurations();
        }

        public List<HardwareConfiguration> getAllHardwareConfigurations(int idCompany)
        {
            return hardwareConfigurationDAO.getAllHardwareConfigurations(idCompany);
        }

        public HardwareConfiguration getHardwareConfiguration(int idHardwareConfiguration)
        {
            return hardwareConfigurationDAO.getHardwareConfiguration(idHardwareConfiguration);
        }

    }
}
