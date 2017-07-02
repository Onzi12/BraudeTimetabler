using System;
using System.Collections;

namespace Api
{
    public class MaxGapBetweenClassesConstraint : ConstraintBase
    {
        private int maxGap;
        public MaxGapBetweenClassesConstraint(int maxGap, bool isEnabled = true) : base(isEnabled)
        {
            this.maxGap = maxGap;
        }

        public override string Name => "Max Gap";
        public override bool IsConstraintSatisfied(Timetable timetable)
        {
            var timeSlots = timetable.TimeSlotsTimetable;

            var timetableMaxGap = 0;
            for (int i = 0; i < Time.TotalSchoolDaysInWeek; i++)
            {
                var dayGap = 0;
                var start = false;
                for (int j = 0; j < Time.TotalHoursOfDay; j++)
                {
                    if (timeSlots[j, i] != null)
                    {
                        timetableMaxGap = Math.Max(timetableMaxGap, dayGap);
                        dayGap = 0;
                        start = true;
                    }
                    else if (start)
                    {
                        ++dayGap;
                    }
                }
            }

            return timetableMaxGap <= this.maxGap;
        }
    }
}