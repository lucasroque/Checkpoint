using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class MarkingDAO
    {
        EmployeeControl employeeControl = new EmployeeControl();

        public Boolean saveMarking(Marking marking)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String pisPasep = marking.pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

            cmd.CommandText = "INSERT INTO MARKING (NSR, MARKING_COUNTER, PIS_PASEP, MARKING_DAY, MARKING_MONTH, " +
                "MARKING_YEAR, MARKING_HOUR, MARKING_MINUTE) VALUES (?,?,?,?,?,?,?,?)";

            cmd.Parameters.Add("NSR", OleDbType.Integer).Value = marking.nsr;
            cmd.Parameters.Add("MARKING_COUNTER", OleDbType.Integer).Value = marking.cont;
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
            cmd.Parameters.Add("MARKING_DAY", OleDbType.Integer).Value = marking.day;
            cmd.Parameters.Add("MARKING_MONTH", OleDbType.Integer).Value = marking.month;
            cmd.Parameters.Add("MARKING_YEAR", OleDbType.Integer).Value = marking.year;
            cmd.Parameters.Add("MARKING_HOUR", OleDbType.Integer).Value = marking.hour;
            cmd.Parameters.Add("MARKING_MINUTE", OleDbType.Integer).Value = marking.minute;

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

        public Boolean saveRejectedMarking(RejectedMarking rejectedMarking)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String pisPasep = rejectedMarking.pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

            cmd.CommandText = "INSERT INTO REJECTED_MARKING (NSR, MARKING_COUNTER, PIS_PASEP, MARKING_DAY, MARKING_MONTH, " +
                "MARKING_YEAR, MARKING_HOUR, MARKING_MINUTE, DESCRIPTION) VALUES (?,?,?,?,?,?,?,?)";

            cmd.Parameters.Add("NSR", OleDbType.Integer).Value = rejectedMarking.nsr;
            cmd.Parameters.Add("MARKING_COUNTER", OleDbType.Integer).Value = rejectedMarking.cont;
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
            cmd.Parameters.Add("MARKING_DAY", OleDbType.Integer).Value = rejectedMarking.day;
            cmd.Parameters.Add("MARKING_MONTH", OleDbType.Integer).Value = rejectedMarking.month;
            cmd.Parameters.Add("MARKING_YEAR", OleDbType.Integer).Value = rejectedMarking.year;
            cmd.Parameters.Add("MARKING_HOUR", OleDbType.Integer).Value = rejectedMarking.hour;
            cmd.Parameters.Add("MARKING_MINUTE", OleDbType.Integer).Value = rejectedMarking.minute;
            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = rejectedMarking.description;

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

        public Boolean updateMarking(Marking marking)
        {
            Boolean success = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            String pisPasep = marking.pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

            cmd.CommandText = "UPDATE MARKING SET NSR=?, MARKING_COUNTER=?, PIS_PASEP=?, MARKING_DAY=?, MARKING_MONTH=?, MARKING_YEAR=?, MARKING_HOUR=?, MARKING_MINUTE=? WHERE ID_MARKING=?";

            cmd.Parameters.Add("NSR", OleDbType.Integer).Value = marking.nsr;
            cmd.Parameters.Add("MARKING_COUNTER", OleDbType.Integer).Value = marking.cont;
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
            cmd.Parameters.Add("MARKING_DAY", OleDbType.Integer).Value = marking.day;
            cmd.Parameters.Add("MARKING_MONTH", OleDbType.Integer).Value = marking.month;
            cmd.Parameters.Add("MARKING_YEAR", OleDbType.Integer).Value = marking.year;
            cmd.Parameters.Add("MARKING_HOUR", OleDbType.Integer).Value = marking.hour;
            cmd.Parameters.Add("MARKING_MINUTE", OleDbType.Integer).Value = marking.minute;
            cmd.Parameters.Add("ID_MARKING", OleDbType.Integer).Value = marking.idMarking;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean updateJustificationMarking(Marking marking)
        {
            Boolean success = true;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE MARKING SET ID_JUSTIFICATION=? WHERE ID_MARKING=?";

            cmd.Parameters.Add("ID_JUSTIFICATION", OleDbType.Integer).Value = marking.idJustification;
            cmd.Parameters.Add("ID_MARKING", OleDbType.Integer).Value = marking.idMarking;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
                success = false;
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public List<DailyMarking> getDailyMarkings(Company company, Department department, Office office, Employee employee, Schedule schedule, DateTime date)
        {
            List<DailyMarking> dailyMarkings = new List<DailyMarking>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            string commandText = "SELECT E.ID_EMPLOYEE, E.PIS_PASEP, M.ID_MARKING, M.NSR, M.MARKING_COUNTER, M.PIS_PASEP, M.MARKING_DAY, M.MARKING_MONTH, M.MARKING_YEAR, M.MARKING_HOUR, M.MARKING_MINUTE FROM EMPLOYEE E LEFT JOIN MARKING M ON E.PIS_PASEP = M.PIS_PASEP ";
            
            Boolean where = false;

            if (company != null && !company.companyName.Equals("Todos"))
            {
                commandText += " WHERE ID_COMPANY=?";
                cmd.Parameters.Add("ID_COMPANY", OleDbType.Integer).Value = company.idCompany;
                where = true;
            }

            if (department != null && !department.description.Equals("Todos"))
            {
                commandText += where ? " AND" : " WHERE";
                commandText += " ID_DEPARTMENT=?";
                cmd.Parameters.Add("ID_DEPARTMENT", OleDbType.Integer).Value = department.idDepartment;
                where = true;
            }

            if (office != null && !office.description.Equals("Todos"))
            {
                commandText += where ? " AND" : " WHERE";
                commandText += " ID_OFFICE=?";
                cmd.Parameters.Add("ID_OFFICE", OleDbType.Integer).Value = office.idOffice;
                where = true;
            }

            if (employee != null && !employee.employeeName.Equals("Todos"))
            {
                String pisPasep = employee.pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

                commandText += where ? " AND" : " WHERE";
                commandText += " E.PIS_PASEP=?";
                cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;
                where = true;
            }

            if (schedule != null && !schedule.description.Equals("Todos"))
            {
                commandText += where ? " AND" : " WHERE";
                commandText += " E.ID_SCHEDULE=?";
                cmd.Parameters.Add("E.ID_SCHEDULE", OleDbType.Integer).Value = schedule.idSchedule;
                where = true;
            }

            commandText += where ? " AND" : " WHERE";
            commandText += " ID_JUSTIFICATION IS NULL";
            commandText += " ORDER BY M.PIS_PASEP, M.MARKING_HOUR, M.MARKING_MINUTE";

            cmd.CommandText = commandText;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                string lastPisPasep = "-";
                DailyMarking dailyMarking;
                List<Marking> markings = null;

                while (result.Read())
                {
                    if (lastPisPasep.Equals(result.GetString(1)))
                    {
                        if (date.Day == Convert.ToInt16(result[6]) && date.Month == Convert.ToInt16(result[7]) && date.Year == Convert.ToInt16(result[8]))
                        {
                            Marking marking = new Marking();
                            marking.idMarking = result[2] == DBNull.Value ? 0 : Convert.ToInt32(result[2]);
                            marking.nsr = result[3] == DBNull.Value ? 0 : Convert.ToInt64(result[3]);
                            marking.cont = result[4] == DBNull.Value ? 0 : Convert.ToInt64(result[4]);
                            marking.pisPasep = result[5] == DBNull.Value ? "" : Formatter.getInstance.formatPisPasep(result.GetString(5), "");
                            marking.day = result[6] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[6]);
                            marking.month = result[7] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[7]);
                            marking.year = result[8] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[8]);
                            marking.hour = result[9] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[9]);
                            marking.minute = result[10] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[10]);
                            markings.Add(marking);
                        }
                    }
                    else
                    {
                        dailyMarking = new DailyMarking();
                        markings = new List<Marking>();
                        Marking marking = new Marking();

                        dailyMarking.employee = employeeControl.getEmployee(Convert.ToInt32(result[0]));
                        dailyMarking.date = date;
                        dailyMarking.markings = markings;

                        if (result[2] != DBNull.Value && date.Day == Convert.ToInt16(result[6]) && date.Month == Convert.ToInt16(result[7]) && date.Year == Convert.ToInt16(result[8]))
                        {
                            marking.idMarking = result[2] == DBNull.Value ? 0 : Convert.ToInt32(result[2]);
                            marking.nsr = result[3] == DBNull.Value ? 0 : Convert.ToInt64(result[3]);
                            marking.cont = result[4] == DBNull.Value ? 0 : Convert.ToInt64(result[4]);
                            marking.pisPasep = result[5] == DBNull.Value ? "" : Formatter.getInstance.formatPisPasep(result.GetString(5), "");
                            marking.day = result[6] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[6]);
                            marking.month = result[7] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[7]);
                            marking.year = result[8] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[8]);
                            marking.hour = result[9] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[9]);
                            marking.minute = result[10] == DBNull.Value ? (Int16)0 : Convert.ToInt16(result[10]);
                            markings.Add(marking);
                        }

                        dailyMarkings.Add(dailyMarking);
                        lastPisPasep = result.GetString(1);
                    }
                }
            }

            result.Close();

            return dailyMarkings;
        }

        public List<DailyMarking> getMarkingCard(Employee employee, DateTime startDate, DateTime endDate)
        {
            List<DailyMarking> dailyMarkings = new List<DailyMarking>();

            String pisPasep = employee.pisPasep.Replace(".", "").Replace("-", "").Replace(" ", "");

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            string commandText = "SELECT M.ID_MARKING, M.NSR, M.MARKING_COUNTER, M.PIS_PASEP, M.MARKING_DAY, M.MARKING_MONTH, M.MARKING_YEAR, M.MARKING_HOUR, M.MARKING_MINUTE FROM MARKING M "
                + "WHERE M.MARKING_DAY >= ? AND M.MARKING_DAY <= ? "
                + "AND M.MARKING_MONTH >= ? AND M.MARKING_MONTH <= ? "
                + "AND M.MARKING_YEAR >= ? AND M.MARKING_YEAR <=? "
                + "AND M.PIS_PASEP = ? AND ID_JUSTIFICATION IS NULL "
                + "ORDER BY M.MARKING_DAY, M.MARKING_MONTH, M.MARKING_YEAR, M.MARKING_HOUR, M.MARKING_MINUTE";

            cmd.Parameters.Add("MARKING_DAY", OleDbType.Integer).Value = startDate.Day;
            cmd.Parameters.Add("MARKING_DAY", OleDbType.Integer).Value = endDate.Day;
            cmd.Parameters.Add("MARKING_MONTH", OleDbType.Integer).Value = startDate.Month;
            cmd.Parameters.Add("MARKING_MONTH", OleDbType.Integer).Value = endDate.Month;
            cmd.Parameters.Add("MARKING_YEAR", OleDbType.Integer).Value = startDate.Year;
            cmd.Parameters.Add("MARKING_YEAR", OleDbType.Integer).Value = endDate.Year;
            cmd.Parameters.Add("PIS_PASEP", OleDbType.VarChar).Value = pisPasep;

            cmd.CommandText = commandText;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                int lastDay = -1;
                DailyMarking dailyMarking;
                List<Marking> markings = null;

                while (result.Read())
                {
                    if (lastDay == Convert.ToInt16(result[4]))
                    {
                        Marking marking = new Marking();
                        marking.idMarking = Convert.ToInt32(result[0]);
                        marking.nsr = Convert.ToInt64(result[1]);
                        marking.cont = Convert.ToInt64(result[2]);
                        marking.pisPasep = Formatter.getInstance.formatPisPasep(result.GetString(3), "");
                        marking.day = Convert.ToInt16(result[4]);
                        marking.month = Convert.ToInt16(result[5]);
                        marking.year = Convert.ToInt16(result[6]);
                        marking.hour = Convert.ToInt16(result[7]);
                        marking.minute = Convert.ToInt16(result[8]);
                        markings.Add(marking);

                    }
                    else
                    {
                        dailyMarking = new DailyMarking();
                        markings = new List<Marking>();
                        Marking marking = new Marking();

                        marking.idMarking = Convert.ToInt32(result[0]);
                        marking.nsr = Convert.ToInt64(result[1]);
                        marking.cont = Convert.ToInt64(result[2]);
                        marking.pisPasep = Formatter.getInstance.formatPisPasep(result.GetString(3), ""); ;
                        marking.day = Convert.ToInt16(result[4]);
                        marking.month = Convert.ToInt16(result[5]);
                        marking.year = Convert.ToInt16(result[6]);
                        marking.hour = Convert.ToInt16(result[7]);
                        marking.minute = Convert.ToInt16(result[8]);
                        markings.Add(marking);

                        dailyMarking.date = new DateTime(marking.year, marking.month, marking.day);
                        dailyMarking.markings = markings;
                        dailyMarking.employee = employee;

                        dailyMarkings.Add(dailyMarking);
                        lastDay = marking.day;
                    }
                }
            }

            result.Close();

            return dailyMarkings;
        }
    }
}
