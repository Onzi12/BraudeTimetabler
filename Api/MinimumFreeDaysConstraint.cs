using System;
using System.Linq;

namespace Api
{
    public class MinimumFreeDaysConstraint : ConstraintBase
    {
        public MinimumFreeDaysConstraint(int minFreeDays) : base(false)
        {
            this.MinFreeDays = minFreeDays;
        }

        public int MinFreeDays
        {
            get; set;
        }

        public override string Name => "Minimum Free Days";

        public override bool IsConstraintSatisfied(Timetable timetable)
        {
            return NumberOfSchoolDays(timetable) <= Time.TotalSchoolDaysInWeek - MinFreeDays;
        }

        public override double GetRateFactor(Timetable timetable)
        {
            var factor = 0;
            for (int i = 0; i < Time.TotalSchoolDaysInWeek; i++)
            {
                for (int j = 0; j < Time.TotalHoursOfDay; j++)
                {
                    if (timetable.TimeSlotsTimetable[j, i]?.Events.Count > 0)
                    {
                       ++factor;
                       break;
                    }
                }
            }

            return factor;
        }

        public override double RatePenalty => 20;

        private int NumberOfSchoolDays(Timetable timetable)
        {
            return timetable.SelectMany(t => t.Events).Select(e => e.Day).Distinct().Count();
        }
    }
}