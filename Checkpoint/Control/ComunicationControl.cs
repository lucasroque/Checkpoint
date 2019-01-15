using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Data.OleDb;

namespace Checkpoint.Control
{
    class ComunicationControl
    {
        HardwareControl hardwareControl = new HardwareControl();

        public static String ip = "189.5.96.23";
        public static String port = "1001";

        public Boolean insertComunication()
        {
            return true;
        }

        public Comunication getComunication()
        {
            Comunication comunication = null;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM COMUNICATION";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    comunication = new Comunication();
                    comunication.idComunication = result.GetInt32(0);
                    comunication.hardware = hardwareControl.getHardware(result.GetInt32(1));
                    comunication.ip = result.GetString(2);
                    comunication.port = result.GetString(3);
                    comunication.endDev = result.GetString(4);
                }
            }

            result.Close();

            return comunication;
        }
    }
}
