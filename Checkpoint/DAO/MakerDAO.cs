using Checkpoint.Model;
using Checkpoint.Tools;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class MakerDAO
    {
        public List<Maker> getAllMakers()
        {
            List<Maker> makers = new List<Maker>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM MAKER";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Maker maker = new Maker();
                    maker.idMaker = result.GetInt32(0);
                    maker.description = result.GetString(1);

                    makers.Add(maker);
                }
            }

            result.Close();

            return makers;
        }

        public Maker getMaker(int idMaker)
        {
            Maker maker = new Maker();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM MAKER WHERE ID_MAKER=?";
            cmd.Parameters.Add("ID_MAKER", OleDbType.Integer).Value = idMaker;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    maker.idMaker = result.GetInt32(0);
                    maker.description = result.GetString(1);
                }
            }

            result.Close();

            return maker;
        }
    }
}
