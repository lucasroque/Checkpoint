using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using System.Windows.Media.Imaging;

namespace Checkpoint.DAO
{
    class HardwareDAO
    {
        MakerControl makerControl = new MakerControl();

        public Boolean saveHardware(Hardware hardware)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String fileName = "hardware_" + hardware.description + ".jpg";

            cmd.CommandText = "INSERT INTO HARDWARE (ID_MAKER, DESCRIPTION, HARDWARE_IMAGE) VALUES (?,?,?)";

            cmd.Parameters.Add("ID_MAKER", OleDbType.Integer).Value = hardware.maker != null ? hardware.maker.idMaker : 0;
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = hardware.description;
            cmd.Parameters.Add("HARDWARE_IMAGE", OleDbType.VarChar).Value = fileName;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;

                String origin = hardware.hardwareImage.UriSource.AbsolutePath.Replace("%20", " ");
                String destiny = ConfigControl.Instance.getImageDirectory() + "/" + fileName;
                destiny = destiny.Replace("\\", "/");

                if (!origin.Equals(destiny))
                {
                    File.Copy(origin, destiny, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao salvar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean updateHardware(Hardware hardware)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String fileName = "hardware_" + hardware.description + ".jpg";

            cmd.CommandText = "UPDATE HARDWARE SET ID_MAKER=?, DESCRIPTION=?, HARDWARE_IMAGE=? WHERE ID_HARDWARE=?";

            cmd.Parameters.Add("ID_MAKER", OleDbType.Integer).Value = hardware.maker != null ? hardware.maker.idMaker : 0;
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = hardware.description;
            cmd.Parameters.Add("HARDWARE_IMAGE", OleDbType.VarChar).Value = fileName;
            cmd.Parameters.Add("ID_HARDWARE", OleDbType.Integer).Value = hardware.idHardware;

            try
            {
                cmd.ExecuteNonQuery();
                success = true;

                String origin = hardware.hardwareImage.UriSource.AbsolutePath.Replace("%20", " ");
                String destiny = ConfigControl.Instance.getImageDirectory() + "/" + fileName;
                destiny = destiny.Replace("\\", "/");

                if (!origin.Equals(destiny))
                {
                    File.Copy(origin, destiny, true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean deleteHardware(Hardware hardware)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM HARDWARE WHERE ID_HARDWARE=?";
            cmd.Parameters.Add("ID_HARDWARE", OleDbType.Integer).Value = hardware.idHardware;

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

        public List<Hardware> getAllHardwares()
        {
            List<Hardware> hardwares = new List<Hardware>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM HARDWARE";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Hardware hardware = new Hardware();
                    hardware.idHardware = result.GetInt32(0);
                    hardware.description = Convert.ToString(result[1]);
                    hardware.maker = makerControl.getMaker(Convert.ToInt32(result[2]));
                    String fileName = Convert.ToString(result[3]);

                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(imageFile);
                        img.EndInit();

                        hardware.hardwareImage = img;
                    }
                    else
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(ConfigControl.Instance.getNoImageFile());
                        img.EndInit();

                        hardware.hardwareImage = img;
                    }

                    hardwares.Add(hardware);
                }
            }

            result.Close();

            return hardwares;
        }

        public Hardware getHardware(int idHardware)
        {
            Hardware hardware = new Hardware();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM HARDWARE WHERE ID_HARDWARE=?";
            cmd.Parameters.Add("ID_HARDWARE", OleDbType.Integer).Value = idHardware;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    hardware.idHardware = result.GetInt32(0);
                    hardware.description = Convert.ToString(result[1]);
                    hardware.maker = makerControl.getMaker(Convert.ToInt32(result[2]));
                    String fileName = result.GetString(3);

                    String imageFile = ConfigControl.Instance.getImageDirectory() + "/" + fileName;

                    if (File.Exists(imageFile))
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(imageFile);
                        img.EndInit();

                        hardware.hardwareImage = img;
                    }
                    else
                    {
                        BitmapImage img = new BitmapImage();
                        img.BeginInit();
                        img.CacheOption = BitmapCacheOption.OnLoad;
                        img.UriSource = new Uri(ConfigControl.Instance.getNoImageFile());
                        img.EndInit();

                        hardware.hardwareImage = img;
                    }
                }
            }

            result.Close();

            return hardware;
        }
    }
}
