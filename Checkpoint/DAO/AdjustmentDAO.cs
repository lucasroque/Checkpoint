using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Data.OleDb;


namespace Checkpoint.DAO
{
    class AdjustmentDAO
    {

        public Boolean saveAdjustment(Adjustment adjustment)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO ADJUSTMENT (ADJ_LAST_MARKING_NSR) VALUES (?)";
            cmd.Parameters.Add("ADJ_LAST_MARKING_NSR", OleDbType.Integer).Value = adjustment.adjLastMarkingNsr;

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

        public Boolean updateAdjustment(Adjustment adjustment)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE ADJUSTMENT SET ADJ_LAST_MARKING_NSR=? WHERE ID_ADJUSTMENT=?";
            cmd.Parameters.Add("ADJ_LAST_MARKING_NSR", OleDbType.Integer).Value = adjustment.adjLastMarkingNsr;
            cmd.Parameters.Add("ID_ADJUSTMENT", OleDbType.Integer).Value = adjustment.idAdjustment;

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

        public Boolean updateLastMarkingNsr(Int64 lastNsr)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE ADJUSTMENT SET ADJ_LAST_MARKING_NSR=?";
            cmd.Parameters.Add("ADJ_LAST_MARKING_NSR", OleDbType.Integer).Value = lastNsr;

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

        public int getGetLastNsr()
        {
            int lastNsr = 0;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM ADJUSTMENT";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    lastNsr = Convert.ToInt32(result[1]);
                }
            }

            result.Close();

            return lastNsr;
        }

        public Adjustment getAdjustment()
        {
            Adjustment adjustment = null;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM ADJUSTMENT";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    adjustment = new Adjustment();
                    adjustment.idAdjustment = result.GetInt32(0);
                    adjustment.adjLastMarkingNsr = result.GetInt32(1);
                }
            }

            result.Close();

            return adjustment;
        }

    }
}
