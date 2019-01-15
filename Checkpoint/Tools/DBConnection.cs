using System;
using System.Data;
using System.Data.OleDb;

namespace Checkpoint.Tools
{
    class DBConnection
    {
        private static DBConnection instance;

        private OleDbConnection conn;

        private DBConnection()
        {
            conn = new OleDbConnection();
            conn.ConnectionString = "Provider=Microsoft.Jet.Oledb.4.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\POKDB.mdb";
        }

        public static DBConnection getInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBConnection();
                }
                return instance;
            }
        }

        public OleDbCommand getDbCommand()
        {
            OleDbCommand cmd = new OleDbCommand();
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
       
            cmd.Connection = conn;
            return cmd;
        }

        public void closeConnection()
        {
            conn.Close();
        }
    }
}
