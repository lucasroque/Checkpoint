using System;
using System.IO;

namespace Checkpoint.Control
{
    class ConfigControl
    {
        public static String SYSTEM_VERSION = "CHECKPOINT 1.17 Beta";

        private String DV_BD_FILE = AppDomain.CurrentDomain.BaseDirectory + "\\POKDB.mdb";
        private String DV_IMAGE_DIRECTORY = @AppDomain.CurrentDomain.BaseDirectory + "saved_images";
        private String DV_BACKUP_DIRECTORY = AppDomain.CurrentDomain.BaseDirectory + "backup";
        private String DV_NO_IMAGE_FILE = "/noimage.jpg";

        private static readonly ConfigControl instance = new ConfigControl();

        public static ConfigControl Instance
        {
            get
            {
                return instance;
            }
        }

        public void createImageDirectories()
        {
            if (!File.Exists(DV_BD_FILE))
            {
                File.WriteAllBytes(DV_BD_FILE, Properties.Resources.POKDB);
            }


            if (!Directory.Exists(DV_IMAGE_DIRECTORY))
            {
                Directory.CreateDirectory(DV_IMAGE_DIRECTORY);
                Properties.Resources.noimage.Save(getNoImageFile());
            }

            if (!Directory.Exists(DV_BACKUP_DIRECTORY))
            {
                Directory.CreateDirectory(DV_BACKUP_DIRECTORY);
            }

        }

        public String getImageDirectory()
        {
            return DV_IMAGE_DIRECTORY;
        }

        public String getBackupDirectory()
        {
            return DV_BACKUP_DIRECTORY;
        }

        public String getNoImageFile()
        {
            return DV_IMAGE_DIRECTORY + DV_NO_IMAGE_FILE;
        }
    }
}
