using Checkpoint.DAO;
using Checkpoint.Model;
using Checkpoint.Tools;
using System;
using System.Collections.Generic;

namespace Checkpoint.Control
{
    class MarkingControl
    {
        ScheduleControl scheduleControl = new ScheduleControl();
        RecessControl recessControl = new RecessControl();
        AbsenceControl absenceControl = new AbsenceControl();

        MarkingDAO markingDAO = new MarkingDAO();

        public Boolean saveMarking(Marking marking)
        {
            return markingDAO.saveMarking(marking);
        }

        public Boolean saveRejectedMarking(RejectedMarking rejectedMarking)
        {
            return markingDAO.saveRejectedMarking(rejectedMarking);
        }

        public Boolean updateMarking(Marking marking)
        {
            return markingDAO.updateMarking(marking);
        }

        public Boolean updateJustificationMarking(Marking marking)
        {
            return markingDAO.updateJustificationMarking(marking);
        }

        public List<DailyMarking> getDailyMarkings(Company company, Department department, Office office, Employee employee, Schedule schedule, DateTime date)
        {

            List<DailyMarking>  dailyMarkings = markingDAO.getDailyMarkings(company, department, office, employee, schedule, date);

            foreach (DailyMarking dm in dailyMarkings)
            {
                calculateHours(dm);
            }

            return dailyMarkings;
        }

        public List<DailyMarking> getMarkingCard(Employee employee, DateTime startDate, DateTime endDate)
        {

            List<DailyMarking> dailyMarkings = markingDAO.getMarkingCard(employee, startDate, endDate);

            Dictionary<DateTime, DailyMarking> dicDates = createDictionaryDates(employee, startDate, endDate);

            foreach (DailyMarking dm in dailyMarkings)
            {
                calculateHours(dm);
                dicDates[dm.date] = dm;
            }

            return convertDicToList(dicDates);
        }

        private Dictionary<DateTime, DailyMarking> createDictionaryDates(Employee employee, DateTime startDate, DateTime endDate)
        {

            Dictionary<DateTime, DailyMarking> dicDates = new Dictionary<DateTime, DailyMarking>();
            DateTime date = startDate;

            while (date.CompareTo(endDate) <= 0)
            {
                DailyMarking dm = new DailyMarking(employee, date);
                calculateHours(dm);

                dicDates.Add(date, dm);
                date = date.AddDays(1);
            }

            return dicDates;
        }

        private List<DailyMarking> convertDicToList(Dictionary<DateTime, DailyMarking> dicDates)
        {
            List<DailyMarking> dailyMarkings = new List<DailyMarking>();

            foreach (DailyMarking day in dicDates.Values)
            {
                dailyMarkings.Add(day);
            }

            return dailyMarkings;
        }

        public void calculateHours(DailyMarking dm)
        {
            setFieldsByMarkings(dm);
            
            List<DateTime> recessDates = recessControl.getAllRecessDates();
            ScheduleDay scheduleDay = scheduleControl.getScheduleDay(dm.employee.schedule, MarkingUtil.getDayKey(dm.date.DayOfWeek));
            Boolean usualDay = true;
            string entryTolerance = "0";
            string exitTolerance = "0";

            TimeSpan standartHoursTS = TimeSpan.Parse("00:00");
            TimeSpan faultHoursTS = TimeSpan.Parse("00:00");
            TimeSpan extraHoursTS = TimeSpan.Parse("00:00");
            TimeSpan workloadTS = TimeSpan.Parse("00:00");

            if(scheduleDay.workload != null && !"".Equals(scheduleDay.workload))
            {
                if (absenceControl.getEmployeeInAbsence(dm.employee, dm.date))
                {
                    if (!MarkingUtil.testRestrictionModification("AFASTADO", dm))
                    {
                        dm.entryOne = "AFASTADO";
                        dm.exitOne = "AFASTADO";
                        dm.entryTwo = "AFASTADO";
                        dm.exitTwo = "AFASTADO";
                        dm.entryThree = "AFASTADO";
                        dm.exitThree = "AFASTADO";
                        return;
                    }
                    else
                    {
                        usualDay = false;
                    }

                }

                if (recessDates.Contains(dm.date))
                {
                    if (!MarkingUtil.testRestrictionModification("FERIADO", dm))
                    {
                        dm.entryOne = "FERIADO";
                        dm.exitOne = "FERIADO";
                        dm.entryTwo = "FERIADO";
                        dm.exitTwo = "FERIADO";
                        dm.entryThree = "FERIADO";
                        dm.exitThree = "FERIADO";
                        return;
                    }
                    else
                    {
                        usualDay = false;
                    }

                }

                if ("FOLGA".Equals(scheduleDay.workload))
                {
                    if (!MarkingUtil.testRestrictionModification("FOLGA", dm))
                    {
                        dm.entryOne = "FOLGA";
                        dm.exitOne = "FOLGA";
                        dm.entryTwo = "FOLGA";
                        dm.exitTwo = "FOLGA";
                        dm.entryThree = "FOLGA";
                        dm.exitThree = "FOLGA";
                        return;
                    }
                    else
                    {
                        usualDay = false;
                    }
                }

                if (usualDay)
                {
                    workloadTS = TimeSpan.Parse(scheduleDay.workload);
                    entryTolerance = scheduleDay.entryTolerance;
                    exitTolerance = scheduleDay.exitTolerance;
                }

                if (dm.entryOne != null && dm.exitOne != null)
                {
                    standartHoursTS += TimeSpan.Parse(dm.exitOne) - TimeSpan.Parse(dm.entryOne);
                }
                if (dm.entryTwo != null && dm.exitTwo != null)
                {
                    standartHoursTS += TimeSpan.Parse(dm.exitTwo) - TimeSpan.Parse(dm.entryTwo);
                }
                if (dm.entryThree != null && dm.exitThree != null)
                {
                    standartHoursTS += TimeSpan.Parse(dm.exitThree) - TimeSpan.Parse(dm.entryThree);
                }

                if (standartHoursTS != TimeSpan.Parse("00:00"))
                {
                    if (scheduleDay.compensation)
                    {
                        if (standartHoursTS.CompareTo(workloadTS) > 0)
                        {
                            TimeSpan extra = standartHoursTS - workloadTS;

                            if (extra.TotalMinutes > Convert.ToInt16(entryTolerance))
                            {
                                extraHoursTS = extra;
                            }
                            else
                            {
                                standartHoursTS = workloadTS;
                            }
                        }
                        else
                        {
                            TimeSpan fault = workloadTS - standartHoursTS;

                            if (fault.TotalMinutes > Convert.ToInt16(exitTolerance))
                            {
                                faultHoursTS = workloadTS - standartHoursTS;
                            }
                            else
                            {
                                standartHoursTS = workloadTS;
                            }
                        }
                    }
                    else
                    {
                        if (standartHoursTS.CompareTo(workloadTS) > 0)
                        {
                            extraHoursTS = standartHoursTS - workloadTS;
                        }
                        else
                        {
                            faultHoursTS = workloadTS - standartHoursTS;
                        }
                    }
                }
                else
                {
                    if (scheduleDay.automaticDayOff)
                    {
                        dm.entryOne = "FOLGA";
                        dm.exitOne = "FOLGA";
                        dm.entryTwo = "FOLGA";
                        dm.exitTwo = "FOLGA";
                        dm.entryThree = "FOLGA";
                        dm.exitThree = "FOLGA";
                        standartHoursTS = TimeSpan.Parse("00:00");
                        faultHoursTS = TimeSpan.Parse("00:00");
                        extraHoursTS = TimeSpan.Parse("00:00");
                        workloadTS = TimeSpan.Parse("00:00");
                        return;
                    }
                }

                dm.standartHours = standartHoursTS.ToString(@"hh\:mm");
                dm.faultHours = faultHoursTS.ToString(@"hh\:mm");
                dm.extraHours = extraHoursTS.ToString(@"hh\:mm");
                dm.workload = workloadTS.ToString(@"hh\:mm");
            }  
        }

        private void setFieldsByMarkings(DailyMarking dm)
        {
            if (dm.markings.Count >= 1)
            {
                Marking mk = dm.markings[0];
                dm.entryOne = Convert.ToString(mk.hour).PadLeft(2, '0') + ":" + Convert.ToString(mk.minute).PadLeft(2, '0');
            }
            if (dm.markings.Count >= 2)
            {
                Marking mk = dm.markings[1];
                dm.exitOne = Convert.ToString(mk.hour).PadLeft(2, '0') + ":" + Convert.ToString(mk.minute).PadLeft(2, '0');
            }
            if (dm.markings.Count >= 3)
            {
                Marking mk = dm.markings[2];
                dm.entryTwo = Convert.ToString(mk.hour).PadLeft(2, '0') + ":" + Convert.ToString(mk.minute).PadLeft(2, '0');
            }
            if (dm.markings.Count >= 4)
            {
                Marking mk = dm.markings[3];
                dm.exitTwo = Convert.ToString(mk.hour).PadLeft(2, '0') + ":" + Convert.ToString(mk.minute).PadLeft(2, '0');
            }
            if (dm.markings.Count >= 5)
            {
                Marking mk = dm.markings[4];
                dm.entryThree = Convert.ToString(mk.hour).PadLeft(2, '0') + ":" + Convert.ToString(mk.minute).PadLeft(2, '0');
            }
            if (dm.markings.Count >= 6)
            {
                Marking mk = dm.markings[5];
                dm.exitThree = Convert.ToString(mk.hour).PadLeft(2, '0') + ":" + Convert.ToString(mk.minute).PadLeft(2, '0');
            }
        }

        public void markingsUpdate(DailyMarking dm)
        {
            if (!MarkingUtil.isEmptyMarking(dm.entryOne))
            {
                string[] time = dm.entryOne.Split(':');

                if (dm.markings.Count >= 1)
                {
                    dm.markings[0].hour = Convert.ToInt16(time[0]);
                    dm.markings[0].minute = Convert.ToInt16(time[1]);
                }
                else
                {
                    dm.markings.Add(new Marking(-1, -1, dm.employee.pisPasep, Convert.ToInt16(dm.date.Day), Convert.ToInt16(dm.date.Month), Convert.ToInt16(dm.date.Year), Convert.ToInt16(time[0]), Convert.ToInt16(time[1])));
                }
            }
            if (!MarkingUtil.isEmptyMarking(dm.exitOne))
            {
                string[] time = dm.exitOne.Split(':');

                if (dm.markings.Count >= 2)
                {
                    dm.markings[1].hour = Convert.ToInt16(time[0]);
                    dm.markings[1].minute = Convert.ToInt16(time[1]);
                }
                else
                {
                    dm.markings.Add(new Marking(-1, -1, dm.employee.pisPasep, Convert.ToInt16(dm.date.Day), Convert.ToInt16(dm.date.Month), Convert.ToInt16(dm.date.Year), Convert.ToInt16(time[0]), Convert.ToInt16(time[1])));
                }
            }
            if (!MarkingUtil.isEmptyMarking(dm.entryTwo))
            {
                string[] time = dm.entryTwo.Split(':');

                if (dm.markings.Count >= 3)
                {
                    dm.markings[2].hour = Convert.ToInt16(time[0]);
                    dm.markings[2].minute = Convert.ToInt16(time[1]);
                }
                else
                {
                    dm.markings.Add(new Marking(-1, -1, dm.employee.pisPasep, Convert.ToInt16(dm.date.Day), Convert.ToInt16(dm.date.Month), Convert.ToInt16(dm.date.Year), Convert.ToInt16(time[0]), Convert.ToInt16(time[1])));
                }
            }
            if (!MarkingUtil.isEmptyMarking(dm.exitTwo))
            {
                string[] time = dm.exitTwo.Split(':');

                if (dm.markings.Count >= 4)
                {
                    dm.markings[3].hour = Convert.ToInt16(time[0]);
                    dm.markings[3].minute = Convert.ToInt16(time[1]);
                }
                else
                {
                    dm.markings.Add(new Marking(-1, -1, dm.employee.pisPasep, Convert.ToInt16(dm.date.Day), Convert.ToInt16(dm.date.Month), Convert.ToInt16(dm.date.Year), Convert.ToInt16(time[0]), Convert.ToInt16(time[1])));
                }
            }
            if (!MarkingUtil.isEmptyMarking(dm.entryThree))
            {
                string[] time = dm.entryThree.Split(':');

                if (dm.markings.Count >= 5)
                {
                    dm.markings[4].hour = Convert.ToInt16(time[0]);
                    dm.markings[4].minute = Convert.ToInt16(time[1]);
                }
                else
                {
                    dm.markings.Add(new Marking(-1, -1, dm.employee.pisPasep, Convert.ToInt16(dm.date.Day), Convert.ToInt16(dm.date.Month), Convert.ToInt16(dm.date.Year), Convert.ToInt16(time[0]), Convert.ToInt16(time[1])));
                }
            }
            if (!MarkingUtil.isEmptyMarking(dm.exitThree))
            {
                string[] time = dm.exitThree.Split(':');

                if (dm.markings.Count >= 6)
                {
                    dm.markings[5].hour = Convert.ToInt16(time[0]);
                    dm.markings[5].minute = Convert.ToInt16(time[1]);
                }
                else
                {
                    dm.markings.Add(new Marking(-1, -1, dm.employee.pisPasep, Convert.ToInt16(dm.date.Day), Convert.ToInt16(dm.date.Month), Convert.ToInt16(dm.date.Year), Convert.ToInt16(time[0]), Convert.ToInt16(time[1])));
                }
            }
        }

        public String dailyMarkingValidate(DailyMarking dm)
        {
            if (!MarkingUtil.isEmptyMarking(dm.entryOne) && !MarkingUtil.isEmptyMarking(dm.exitOne))
            {
                if (TimeSpan.Parse(dm.exitOne).CompareTo(TimeSpan.Parse(dm.entryOne)) == -1)
                {
                    return "Saída 1 menor que Entrada 1";
                }
            }

            if (!MarkingUtil.isEmptyMarking(dm.exitOne) && !MarkingUtil.isEmptyMarking(dm.entryTwo))
            {
                if (TimeSpan.Parse(dm.entryTwo).CompareTo(TimeSpan.Parse(dm.exitOne)) == -1)
                {
                    return "Entrada 2 menor que Saída 1";
                }
            }

            if (!MarkingUtil.isEmptyMarking(dm.exitTwo) && !MarkingUtil.isEmptyMarking(dm.entryTwo))
            {
                if (TimeSpan.Parse(dm.exitTwo).CompareTo(TimeSpan.Parse(dm.entryTwo)) == -1)
                {
                    return "Saída 2 menor que Entrada 2";
                }
            }

            if (!MarkingUtil.isEmptyMarking(dm.entryThree) && !MarkingUtil.isEmptyMarking(dm.exitTwo))
            {
                if (TimeSpan.Parse(dm.entryThree).CompareTo(TimeSpan.Parse(dm.exitTwo)) == -1)
                {
                    return "Entrada 3 menor que Saída 2";
                }
            }

            if (!MarkingUtil.isEmptyMarking(dm.exitThree) && !MarkingUtil.isEmptyMarking(dm.entryThree))
            {
                if (TimeSpan.Parse(dm.exitThree).CompareTo(TimeSpan.Parse(dm.entryThree)) == -1)
                {
                    return "Saída 3 menor que Entrada 3";
                }
            }

            return "";
        }
    }
}
