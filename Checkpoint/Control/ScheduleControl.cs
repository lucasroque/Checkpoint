using Checkpoint.DAO;
using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class ScheduleControl
    {
        ScheduleDAO scheduleDAO = new ScheduleDAO();

        public List<ScheduleDay> getNewScheduleDays()
        {
            List<ScheduleDay> newSchedule = new List<ScheduleDay>();
            newSchedule.Add(new ScheduleDay(1, "Segunda-Feira", "5", "5", false));
            newSchedule.Add(new ScheduleDay(2, "Terça-Feira", "5", "5", false));
            newSchedule.Add(new ScheduleDay(3, "Quarta-Feira", "5", "5", false));
            newSchedule.Add(new ScheduleDay(4, "Quinta-Feira", "5", "5", false));
            newSchedule.Add(new ScheduleDay(5, "Sexta-Feira", "5", "5", false));
            newSchedule.Add(new ScheduleDay(6, "Sábado", "5", "5", false));
            newSchedule.Add(new ScheduleDay(0, "Domingo", "5", "5", false));

            return newSchedule;
        }

        public ScheduleDay getScheduleDay(Schedule schedule, int dayKey)
        {
            ScheduleDay scheduleDay = new ScheduleDay();

            foreach (ScheduleDay sd in schedule.scheduleDays)
            {
                if (sd.day_key == dayKey)
                {
                    scheduleDay = sd;
                    break;
                }
            }

            return scheduleDay;
        }

        public int getLastScheduleId()
        {
            return scheduleDAO.getLastScheduleId();
        }

        public Boolean saveSchedule(Schedule schedule)
        {
            return scheduleDAO.saveSchedule(schedule);
        }

        public Boolean updateSchedule(Schedule schedule)
        {
            return scheduleDAO.updateSchedule(schedule);
        }

        public Boolean deleteSchedule(Schedule schedule)
        {
            return scheduleDAO.deleteSchedule(schedule);
        }

        public List<Schedule> getAllSchedules()
        {
            return scheduleDAO.getAllSchedules();
        }

        public List<Schedule> getAllSchedulesControl()
        {
            List<Schedule> allSchedules = scheduleDAO.getAllSchedules();
            allSchedules.Insert(0, new Schedule(0, "Todos"));
            return allSchedules;
        }

        public Schedule getSchedule(int idSchedule)
        {
            return scheduleDAO.getSchedule(idSchedule);
        }

    }
}
