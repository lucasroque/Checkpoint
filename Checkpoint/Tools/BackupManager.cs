using System;
using System.IO;

namespace Checkpoint.Tools
{
    class BackupManager
    {

        private static BackupManager instance;

        public static BackupManager getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new BackupManager();
                }
                return instance;
            }
        }

        public void DoBackup(String backupPath)
        {
            string dbPath = AppDomain.CurrentDomain.BaseDirectory+"//POKDB.mdb";
            string dbFileName = "POKDB.mdb";

            string backupFileName = backupPath + "//" + Path.GetFileNameWithoutExtension(dbFileName) + "_" + DateTime.Now.ToString("yyyy-MM-dd") + Path.GetExtension(dbFileName);
            File.Copy(dbPath, backupFileName, true);
        }

        public void loadBackup(String backupFile)
        {
            string dbPath = AppDomain.CurrentDomain.BaseDirectory + "//POKDB.mdb";
            File.Copy(backupFile, dbPath, true);
        }
    }
}
