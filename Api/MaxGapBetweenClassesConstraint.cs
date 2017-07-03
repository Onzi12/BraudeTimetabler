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

        public override double GetRateFactor(Timetable timetable)
        {
            var timeSlots = timetable.TimeSlotsTimetable;

            double factor = 0;
            for (int i = 0; i < Time.TotalSchoolDaysInWeek; i++)
            {
                double currentGapLength = 0;
                double currentGapPenalty = 0;
                var start = false;
                for (int j = 0; j < Time.TotalHoursOfDay; j++)
                {
                    if (timeSlots[j, i] != null)
                    {
                        factor += currentGapPenalty;
                        currentGapLength = currentGapPenalty = 0;
                        start = true;
                    }
                    else if (start)
                    {
                       ++currentGapLength;
                        currentGapPenalty = Math.Pow(currentGapLength, 2); // gaps longer than 1 receive exponential factor. 1 get 1, 2 get 4, 3 get 9 and so on...
                    }
                }
            }
            return factor;
        }

        public override double RatePenalty => 5;
    }
}