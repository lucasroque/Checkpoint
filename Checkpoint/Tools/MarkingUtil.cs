using Checkpoint.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.Tools
{
    public static class MarkingUtil
    {
        public static int getDayKey(DayOfWeek dayOfWeek)
        {
            if (dayOfWeek == DayOfWeek.Monday)
            {
                return 1;
            }
            else if (dayOfWeek == DayOfWeek.Tuesday)
            {
                return 2;
            }
            else if (dayOfWeek == DayOfWeek.Wednesday)
            {
                return 3;
            }
            else if (dayOfWeek == DayOfWeek.Thursday)
            {
                return 4;
            }
            else if (dayOfWeek == DayOfWeek.Friday)
            {
                return 5;
            }
            else if (dayOfWeek == DayOfWeek.Saturday)
            {
                return 6;
            }
            else if (dayOfWeek == DayOfWeek.Sunday)
            {
                return 0;
            }

            return 0;
        }

        public static Boolean isNullMarking(string marking)
        {
            if (marking == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean isEmptyMarking(string marking)
        {
            if (marking == null || "".Equals(marking) || "FOLGA".Equals(marking) || "AFASTADO".Equals(marking) || "FERIADO".Equals(marking))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Boolean testRestrictionModification(string restriction, DailyMarking dm)
        {
            Boolean modified = false;

            if(!restriction.Equals(dm.entryOne) && dm.entryOne != null)
            {
                modified = true;
            }
            else if (!restriction.Equals(dm.exitOne) && dm.exitOne != null)
            {
                modified = true;
            }
            else if (!restriction.Equals(dm.entryTwo) && dm.entryTwo != null)
            {
                modified = true;
            }
            else if (!restriction.Equals(dm.exitTwo) && dm.exitTwo != null)
            {
                modified = true;
            }
            else if (!restriction.Equals(dm.entryThree) && dm.entryThree != null)
            {
                modified = true;
            }
            else if (!restriction.Equals(dm.exitThree) && dm.exitThree != null)
            {
                modified = true;
            }

            return modified;
        }
    }
}
