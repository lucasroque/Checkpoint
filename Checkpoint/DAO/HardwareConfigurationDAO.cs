using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class HardwareConfigurationDAO
    {
        CompanyControl companyControl = new CompanyControl();
        HardwareControl hardwareControl = new HardwareControl();

        public Boolean saveHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO HARDWARE_CONFIGURATION (ID_COMPANY, ID_HARDWARE, CRYPTOGRAPHIC_KEY, SERIAL_NUMBER, MODEL, VERSION, PORT, IP, CPF) VALUES (?,?,?,?,?,?,?,?,?)";

            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = hardwareConfiguration.company != null ? hardwareConfiguration.company.idCompany : 0;
            cmd.Parameters.Add("ID_HARDWARE", OleDbType.Integer).Value = hardwareConfiguration.hardware != null ? hardwareConfiguration.hardware.idHardware : 0;
            cmd.Parameters.Add("CRYPTOGRAPHIC_KEY", OleDbType.VarChar).Value = hardwareConfiguration.cryptographicKey;
            cmd.Parameters.Add("SERIAL_NUMBER", OleDbType.VarChar).Value = hardwareConfiguration.serialNumber;
            cmd.Parameters.Add("MODEL", OleDbType.VarChar).Value = hardwareConfiguration.model;
            cmd.Parameters.Add("VERSION", OleDbType.VarChar).Value = hardwareConfiguration.version;
            cmd.Parameters.Add("PORT", OleDbType.VarChar).Value = hardwareConfiguration.port;
            cmd.Parameters.Add("IP", OleDbType.VarChar).Value = hardwareConfiguration.ip;
            cmd.Parameters.Add("CPF", OleDbType.VarChar).Value = hardwareConfiguration.cpf;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao salvar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean updateHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE HARDWARE_CONFIGURATION SET ID_COMPANY=?, ID_HARDWARE=?, CRYPTOGRAPHIC_KEY=?, SERIAL_NUMBER=?, MODEL=?, VERSION=?, PORT=?, IP=?, CPF=? WHERE ID_HARDWARE_CONFIGURATION=?";

            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = hardwareConfiguration.company != null ? hardwareConfiguration.company.idCompany : 0;
            cmd.Parameters.Add("ID_HARDWARE", OleDbType.Integer).Value = hardwareConfiguration.hardware != null ? hardwareConfiguration.hardware.idHardware : 0;
            cmd.Parameters.Add("CRYPTOGRAPHIC_KEY", OleDbType.VarChar).Value = hardwareConfiguration.cryptographicKey;
            cmd.Parameters.Add("SERIAL_NUMBER", OleDbType.VarChar).Value = hardwareConfiguration.serialNumber;
            cmd.Parameters.Add("MODEL", OleDbType.VarChar).Value = hardwareConfiguration.model;
            cmd.Parameters.Add("VERSION", OleDbType.VarChar).Value = hardwareConfiguration.version;
            cmd.Parameters.Add("PORT", OleDbType.VarChar).Value = hardwareConfiguration.port;
            cmd.Parameters.Add("IP", OleDbType.VarChar).Value = hardwareConfiguration.ip;
            cmd.Parameters.Add("CPF", OleDbType.VarChar).Value = hardwareConfiguration.cpf;
            cmd.Parameters.Add("ID_HARDWARE_CONFIGURATION", OleDbType.Integer).Value = hardwareConfiguration.idHardwareConfiguration;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean deleteHardwareConfiguration(HardwareConfiguration hardwareConfiguration)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM HARDWARE_CONFIGURATION WHERE ID_HARDWARE_CONFIGURATION=?";
            cmd.Parameters.Add("ID_HARDWARE", OleDbType.Integer).Value = hardwareConfiguration.idHardwareConfiguration;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao excluir!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public List<HardwareConfiguration> getAllHardwareConfigurations()
        {
            List<HardwareConfiguration> hardwareConfigurations = new List<HardwareConfiguration>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM HARDWARE_CONFIGURATION";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    HardwareConfiguration hardwareConfiguration = new HardwareConfiguration();
                    hardwareConfiguration.idHardwareConfiguration = result.GetInt32(0);
                    hardwareConfiguration.company = companyControl.getCompany(result.GetInt32(1));
                    hardwareConfiguration.hardware = hardwareControl.getHardware(result.GetInt32(2));
                    hardwareConfiguration.cryptographicKey = result.GetString(3);
                    hardwareConfiguration.serialNumber = result.GetString(4);
                    hardwareConfiguration.model = result.GetString(5);
                    hardwareConfiguration.version = result.GetString(6);
                    hardwareConfiguration.port = result.GetString(7);
                    hardwareConfiguration.ip = result.GetString(8);
                    hardwareConfiguration.cpf = result.GetString(9);

                    hardwareConfigurations.Add(hardwareConfiguration);
                }
            }

            result.Close();

            return hardwareConfigurations;
        }

        public List<HardwareConfiguration> getAllHardwareConfigurations(int idCompany)
        {
            List<HardwareConfiguration> hardwareConfigurations = new List<HardwareConfiguration>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM HARDWARE_CONFIGURATION WHERE ID_COMPANY=?";
            cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = idCompany;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    HardwareConfiguration hardwareConfiguration = new HardwareConfiguration();
                    hardwareConfiguration.idHardwareConfiguration = result.GetInt32(0);
                    hardwareConfiguration.company = companyControl.getCompany(result.GetInt32(1));
                    hardwareConfiguration.hardware = hardwareControl.getHardware(result.GetInt32(2));
                    hardwareConfiguration.cryptographicKey = result.GetString(3);
                    hardwareConfiguration.serialNumber = result.GetString(4);
                    hardwareConfiguration.model = result.GetString(5);
                    hardwareConfiguration.version = result.GetString(6);
                    hardwareConfiguration.port = result.GetString(7);
                    hardwareConfiguration.ip = result.GetString(8);
                    hardwareConfiguration.cpf = result.GetString(9);

                    hardwareConfigurations.Add(hardwareConfiguration);
                }
            }

            result.Close();

            return hardwareConfigurations;
        }

        public HardwareConfiguration getHardwareConfiguration(int idHardwareConfiguration)
        {
            HardwareConfiguration hardwareConfiguration = new HardwareConfiguration();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM HARDWARE_CONFIGURATION WHERE ID_HARDWARE_CONFIGURATION=?";
            cmd.Parameters.Add("ID_HARDWARE_CONFIGURATION", OleDbType.Integer).Value = idHardwareConfiguration;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    hardwareConfiguration.idHardwareConfiguration = result.GetInt32(0);
                    hardwareConfiguration.company = companyControl.getCompany(result.GetInt32(1));
                    hardwareConfiguration.hardware = hardwareControl.getHardware(result.GetInt32(2));
                    hardwareConfiguration.cryptographicKey = result.GetString(3);
                    hardwareConfiguration.serialNumber = result.GetString(4);
                    hardwareConfiguration.model = result.GetString(5);
                    hardwareConfiguration.version = result.GetString(6);
                    hardwareConfiguration.port = result.GetString(7);
                    hardwareConfiguration.ip = result.GetString(8);
                    hardwareConfiguration.cpf = result.GetString(9);

                }
            }

            result.Close();

            return hardwareConfiguration;
        }
    }
}
