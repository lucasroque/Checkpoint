using Checkpoint.Model;
using Checkpoint.Control;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class AbsenceDAO
    {
        EmployeeControl employeeControl = new EmployeeControl();
        JustificationControl justificationControl = new JustificationControl();

        public Boolean saveAbsence(Absence absence)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO ABSENCE (ID_EMPLOYEE, START_DATE, END_DATE, ID_JUSTIFICATION) VALUES (?,?,?,?)";

            cmd.Parameters.Add("ID_EMPLOYEE", OleDbType.Integer).Value = absence.employee != null ? absence.employee.idEmployee : 0;
            cmd.Parameters.Add("START_DATE", OleDbType.Date).Value = absence.startDate;
            cmd.Parameters.Add("END_DATE", OleDbType.Date).Value = absence.endDate;
            cmd.Parameters.Add("ID_JUSTIFICATION", OleDbType.Integer).Value = absence.justification != null ? absence.justification.idJustification : 0;

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

        public Boolean updateAbsence(Absence absence)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE ABSENCE SET ID_EMPLOYEE=?, START_DATE=?,  END_DATE=?, ID_JUSTIFICATION=? WHERE ID_ABSENCE=?";

            cmd.Parameters.Add("ID_EMPLOYEE", OleDbType.Integer).Value = absence.employee != null ? absence.employee.idEmployee : 0;
            cmd.Parameters.Add("START_DATE", OleDbType.Date).Value = absence.startDate;
            cmd.Parameters.Add("END_DATE", OleDbType.Date).Value = absence.endDate;
            cmd.Parameters.Add("ID_JUSTIFICATION", OleDbType.Integer).Value = absence.justification != null ? absence.justification.idJustification : 0;
            cmd.Parameters.Add("ID_ABSENCE", OleDbType.Integer).Value = absence.idAbsence;

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

        public Boolean deleteAbsence(Absence absence)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM ABSENCE WHERE ID_ABSENCE=?";
            cmd.Parameters.Add("ID_ABSENCE", OleDbType.Integer).Value = absence.idAbsence;

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

        public List<Absence> getAllAbsence()
        {
            List<Absence> absences = new List<Absence>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM ABSENCE";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Absence absence = new Absence();
                    absence.idAbsence = result.GetInt32(0);
                    absence.employee = employeeControl.getEmployee(result.GetInt32(1));
                    absence.startDate = result.GetDateTime(2);
                    absence.endDate = result.GetDateTime(3);
                    absence.justification = justificationControl.getJustification(result.GetInt32(4));

                    absences.Add(absence);
                }
            }

            result.Close();

            return absences;
        }

        public Boolean getEmployeeInAbsence(Employee employee, DateTime date)
        {
            Boolean inAbsence = false;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM ABSENCE WHERE ID_EMPLOYEE=?";
            cmd.Parameters.Add("ID_EMPLOYEE", OleDbType.Integer).Value = employee.idEmployee;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    DateTime start = result.GetDateTime(2);
                    DateTime end = result.GetDateTime(3);

                    if (date >= start && date <= end)
                    {
                        inAbsence = true;
                    }                    
                }
            }

            result.Close();

            return inAbsence;
        }

    }
}
