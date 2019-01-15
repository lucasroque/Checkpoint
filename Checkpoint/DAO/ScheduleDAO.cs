using Checkpoint.Control;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;
using System.Data.OleDb;

namespace Checkpoint.DAO
{
    class ScheduleDAO
    {
        CompanyControl companyControl = new CompanyControl();

        public int getLastScheduleId()
        {
            int scheduleId = 0;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM SCHEDULE ORDER BY ID_SCHEDULE DESC";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    scheduleId = result.GetInt32(0);
                }
            }

            result.Close();

            return scheduleId;
        }

        public Boolean saveSchedule(Schedule schedule)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO SCHEDULE (DESCRIPTION, NIGHTLY_START, OVERTIME_PERCENTAGE) VALUES (?,?,?)";

            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = schedule.description;
            cmd.Parameters.Add("NIGHTLY_START", OleDbType.VarChar).Value = schedule.nightlyStart;
            cmd.Parameters.Add("OVERTIME_PERCENTAGE", OleDbType.VarChar).Value = schedule.overtimePercentage;

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

            if (success)
            {
                int idSchedule = getLastScheduleId();

                foreach (ScheduleDay sd in schedule.scheduleDays)
                {
                    sd.idSchedule = idSchedule;
                    saveScheduleDay(sd);
                }
            }

            return success;
        }

        public void saveScheduleDay(ScheduleDay scheduleDay)
        {
            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "INSERT INTO SCHEDULE_DAY (ID_SCHEDULE, DAY_KEY, ENTRY_ONE, EXIT_ONE, ENTRY_TWO, EXIT_TWO, ENTRY_THREE, EXIT_THREE, ENTRY_TOLERANCE, EXIT_TOLERANCE, WORKLOAD, END_OFFICE_HOUR, COMPENSATION, AUTOMATIC_DAY_OFF, NEUTRAL) VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";

            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = scheduleDay.idSchedule;
            cmd.Parameters.Add("DAY_KEY", OleDbType.Integer).Value = scheduleDay.day_key;
            cmd.Parameters.Add("ENTRY_ONE", OleDbType.VarChar).Value = scheduleDay.EntryOne != null ? scheduleDay.EntryOne : "";
            cmd.Parameters.Add("EXIT_ONE", OleDbType.VarChar).Value = scheduleDay.exitOne != null ? scheduleDay.exitOne : "";
            cmd.Parameters.Add("ENTRY_TWO", OleDbType.VarChar).Value = scheduleDay.entryTwo != null ? scheduleDay.entryTwo : "";
            cmd.Parameters.Add("EXIT_TWO", OleDbType.VarChar).Value = scheduleDay.exitTwo != null ? scheduleDay.exitTwo : "";
            cmd.Parameters.Add("ENTRY_THREE", OleDbType.VarChar).Value = scheduleDay.entryThree != null ? scheduleDay.entryThree : "";
            cmd.Parameters.Add("EXIT_THREE", OleDbType.VarChar).Value = scheduleDay.exitThree != null ? scheduleDay.exitThree : "";
            cmd.Parameters.Add("ENTRY_TOLERANCE", OleDbType.VarChar).Value = scheduleDay.entryTolerance != null ? scheduleDay.entryTolerance : "";
            cmd.Parameters.Add("EXIT_TOLERANCE", OleDbType.VarChar).Value = scheduleDay.exitTolerance != null ? scheduleDay.exitTolerance : "";
            cmd.Parameters.Add("WORKLOAD", OleDbType.VarChar).Value = scheduleDay.workload != null ? scheduleDay.workload : "";
            cmd.Parameters.Add("END_OFFICE_HOUR", OleDbType.VarChar).Value = scheduleDay.endOfficeHour != null ? scheduleDay.endOfficeHour : "";
            cmd.Parameters.Add("COMPENSATION", OleDbType.Integer).Value = scheduleDay.compensation ? 1 : 0;
            cmd.Parameters.Add("AUTOMATIC_DAY_OFF", OleDbType.Integer).Value = scheduleDay.automaticDayOff ? 1 : 0;
            cmd.Parameters.Add("NEUTRAL", OleDbType.Integer).Value = scheduleDay.neutral ? 1 : 0;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao salvar!" + e);
            }

            DBConnection.getInstance.closeConnection();
        }

        public Boolean updateSchedule(Schedule schedule)
        {
            Boolean success;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE SCHEDULE SET DESCRIPTION=?, NIGHTLY_START=?, OVERTIME_PERCENTAGE=? WHERE ID_SCHEDULE=?";

            cmd.Parameters.Add("DESCRIPTION", OleDbType.VarChar).Value = schedule.description;
            cmd.Parameters.Add("NIGHTLY_START", OleDbType.VarChar).Value = schedule.nightlyStart;
            cmd.Parameters.Add("OVERTIME_PERCENTAGE", OleDbType.VarChar).Value = schedule.overtimePercentage;
            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = schedule.idSchedule;

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

            if (success)
            {
                foreach (ScheduleDay sd in schedule.scheduleDays)
                {
                    updateScheduleDay(sd);
                }
            }

            return success;
        }

        public void updateScheduleDay(ScheduleDay scheduleDay)
        {
            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "UPDATE SCHEDULE_DAY SET ID_SCHEDULE=?, DAY_KEY=?, ENTRY_ONE=?, EXIT_ONE=?, ENTRY_TWO=?, EXIT_TWO=?, ENTRY_THREE=?, EXIT_THREE=?, ENTRY_TOLERANCE=?, EXIT_TOLERANCE=?, WORKLOAD=?, END_OFFICE_HOUR=?, COMPENSATION=?, AUTOMATIC_DAY_OFF=?, NEUTRAL=? WHERE ID_SCHEDULE_DAY=?";

            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = scheduleDay.idSchedule;
            cmd.Parameters.Add("DAY_KEY", OleDbType.Integer).Value = scheduleDay.day_key;
            cmd.Parameters.Add("ENTRY_ONE", OleDbType.VarChar).Value = scheduleDay.EntryOne != null ? scheduleDay.EntryOne : "";
            cmd.Parameters.Add("EXIT_ONE", OleDbType.VarChar).Value = scheduleDay.exitOne != null ? scheduleDay.exitOne : "";
            cmd.Parameters.Add("ENTRY_TWO", OleDbType.VarChar).Value = scheduleDay.entryTwo != null ? scheduleDay.entryTwo : "";
            cmd.Parameters.Add("EXIT_TWO", OleDbType.VarChar).Value = scheduleDay.exitTwo != null ? scheduleDay.exitTwo : "";
            cmd.Parameters.Add("ENTRY_THREE", OleDbType.VarChar).Value = scheduleDay.entryThree != null ? scheduleDay.entryThree : "";
            cmd.Parameters.Add("EXIT_THREE", OleDbType.VarChar).Value = scheduleDay.exitThree != null ? scheduleDay.exitThree : "";
            cmd.Parameters.Add("ENTRY_TOLERANCE", OleDbType.VarChar).Value = scheduleDay.entryTolerance != null ? scheduleDay.entryTolerance : "";
            cmd.Parameters.Add("EXIT_TOLERANCE", OleDbType.VarChar).Value = scheduleDay.exitTolerance != null ? scheduleDay.exitTolerance : "";
            cmd.Parameters.Add("WORKLOAD", OleDbType.VarChar).Value = scheduleDay.workload != null ? scheduleDay.workload : "";
            cmd.Parameters.Add("END_OFFICE_HOUR", OleDbType.VarChar).Value = scheduleDay.endOfficeHour != null ? scheduleDay.endOfficeHour : "";
            cmd.Parameters.Add("COMPENSATION", OleDbType.Integer).Value = scheduleDay.compensation ? 1 : 0;
            cmd.Parameters.Add("AUTOMATIC_DAY_OFF", OleDbType.Integer).Value = scheduleDay.automaticDayOff ? 1 : 0;
            cmd.Parameters.Add("NEUTRAL", OleDbType.Integer).Value = scheduleDay.neutral ? 1 : 0;
            cmd.Parameters.Add("ID_SCHEDULE_DAY", OleDbType.Integer).Value = scheduleDay.idScheduleDay;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao alterar!" + e);
            }

            DBConnection.getInstance.closeConnection();
        }

        public Boolean deleteSchedule(Schedule schedule)
        {
            Boolean success = deleteScheduleDay(schedule);

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM SCHEDULE WHERE ID_SCHEDULE=?";
            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = schedule.idSchedule;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine("Erro ao excluir!" + e);
            }

            DBConnection.getInstance.closeConnection();

            return success;
        }

        public Boolean deleteScheduleDay(Schedule schedule)
        {
            Boolean success = false;

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();

            cmd.CommandText = "DELETE FROM SCHEDULE_DAY WHERE ID_SCHEDULE=?";
            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = schedule.idSchedule;

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

        public List<Schedule> getAllSchedules()
        {
            List<Schedule> schedules = new List<Schedule>();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM SCHEDULE";
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                while (result.Read())
                {
                    Schedule schedule = new Schedule();
                    schedule.idSchedule = result.GetInt32(0);
                    schedule.description = result.GetString(1);
                    schedule.nightlyStart = result.GetString(2);
                    schedule.overtimePercentage = result.GetString(3);

                    List<ScheduleDay> scheduleDays = new List<ScheduleDay>();

                    OleDbCommand cmdSD = DBConnection.getInstance.getDbCommand();
                    cmdSD.CommandText = "SELECT * FROM SCHEDULE_DAY WHERE ID_SCHEDULE=?";
                    cmdSD.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = schedule.idSchedule;
                    OleDbDataReader resultSD = cmdSD.ExecuteReader();

                    while (resultSD.Read())
                    {
                        ScheduleDay sd = new ScheduleDay();
                        sd.idScheduleDay = resultSD.GetInt32(0);
                        sd.idSchedule = resultSD.GetInt32(1);
                        sd.day_key = resultSD.GetInt32(2);
                        sd.descriptionDay = getDayDescription(sd.day_key);
                        sd.EntryOne = resultSD.GetString(3);
                        sd.exitOne = resultSD.GetString(4);
                        sd.entryTwo = resultSD.GetString(5);
                        sd.exitTwo = resultSD.GetString(6);
                        sd.entryThree = resultSD.GetString(7);
                        sd.exitThree = resultSD.GetString(8);
                        sd.entryTolerance = resultSD.GetString(9);
                        sd.exitTolerance = resultSD.GetString(10);
                        sd.workload = resultSD.GetString(11);
                        sd.endOfficeHour = Convert.ToString(resultSD[12]);
                        sd.compensation = resultSD.GetInt32(13) == 1 ? true: false;;
                        sd.automaticDayOff = resultSD.GetInt32(14) == 1 ? true : false;
                        sd.neutral = resultSD.GetInt32(15) == 1 ? true : false;

                        schedule.scheduleDays.Add(sd);
                    }

                    resultSD.Close();
                    schedules.Add(schedule);
                }
            }

            result.Close();

            return schedules;
        }

        public Schedule getSchedule(int idSchedule)
        {
            Schedule schedule = new Schedule();

            OleDbCommand cmd = DBConnection.getInstance.getDbCommand();
            cmd.CommandText = "SELECT * FROM SCHEDULE WHERE ID_SCHEDULE=?";
            cmd.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = idSchedule;
            OleDbDataReader result = cmd.ExecuteReader();

            if (result.HasRows)
            {
                if (result.Read())
                {
                    schedule.idSchedule = result.GetInt32(0);
                    schedule.description = result.GetString(1);
                    schedule.nightlyStart = result.GetString(2);
                    schedule.overtimePercentage = result.GetString(3);

                    List<ScheduleDay> scheduleDays = new List<ScheduleDay>();

                    OleDbCommand cmdSD = DBConnection.getInstance.getDbCommand();
                    cmdSD.CommandText = "SELECT * FROM SCHEDULE_DAY WHERE ID_SCHEDULE=?";
                    cmdSD.Parameters.Add("ID_SCHEDULE", OleDbType.Integer).Value = schedule.idSchedule;
                    OleDbDataReader resultSD = cmdSD.ExecuteReader();

                    while (resultSD.Read())
                    {
                        ScheduleDay sd = new ScheduleDay();
                        sd.idScheduleDay = resultSD.GetInt32(0);
                        sd.idSchedule = resultSD.GetInt32(1);
                        sd.day_key = resultSD.GetInt32(2);
                        sd.descriptionDay = getDayDescription(sd.day_key);
                        sd.EntryOne = resultSD.GetString(3);
                        sd.exitOne = resultSD.GetString(4);
                        sd.entryTwo = resultSD.GetString(5);
                        sd.exitTwo = resultSD.GetString(6);
                        sd.entryThree = resultSD.GetString(7);
                        sd.exitThree = resultSD.GetString(8);
                        sd.entryTolerance = resultSD.GetString(9);
                        sd.exitTolerance = resultSD.GetString(10);
                        sd.workload = resultSD.GetString(11);
                        sd.endOfficeHour = Convert.ToString(resultSD[12]);
                        sd.compensation = resultSD.GetInt32(13) == 1 ? true : false; ;
                        sd.automaticDayOff = resultSD.GetInt32(14) == 1 ? true : false;
                        sd.neutral = resultSD.GetInt32(15) == 1 ? true : false;

                        schedule.scheduleDays.Add(sd);
                    }

                    resultSD.Close();
                }
            }

            result.Close();

            return schedule;
        }

        public String getDayDescription(int key_day)
        {
            if (key_day == 0)
            {
                return "Domingo";
            }
            else if (key_day == 1)
            {
                return "Segunda-Feira";
            }
            else if (key_day == 2)
            {
                return "Terça-Feira";
            }
            else if (key_day == 3)
            {
                return "Quarta-Feira";
            }
            else if (key_day == 4)
            {
                return "Quinta-Feira";
            }
            else if (key_day == 5)
            {
                return "Sexta-Feira";
            }
            else if (key_day == 6)
            {
                return "Sábado";
            }

            return null;
        }
    }
}
