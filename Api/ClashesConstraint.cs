using System;

namespace Api
{
    public class ClashesConstraint : ConstraintBase
    {
        public ClashesConstraint(bool clashesAllowed) : base(false)
        {
            ClashesAllowed = clashesAllowed;
        }

        public bool ClashesAllowed
        {
            get; set;
        }

        public override string Name => "Clashes";

        public override bool IsConstraintSatisfied(Timetable timetable)
        {
            if (ClashesAllowed)
            {
                return true;
            }

            return !IsClashesExist(timetable);
        }

        public override double GetRateFactor(Timetable timetable)
        {
            double factor = 0;
            foreach (var timeSlot in timetable.TimeSlotsTimetable)
            {
                if (timeSlot?.Events.Count > 1)
                {
                    factor += Math.Pow(timeSlot.Events.Count - 1, 2); // penalty for each Clash. if clash is bigger then two (3 events for example), the penalty will be in the higher by a exponent factor
                }
            }

            return factor;
        }

        public override double RatePenalty => 11;

        private static bool IsClashesExist(Timetable timetable)
        {
            var timeSlotsTimetable = timetable.TimeSlotsTimetable;
            foreach (var timeSlot in timeSlotsTimetable)
            {
                if (timeSlot?.Events.Count > 1)
                    return true;
            }

            return false;
        }
    }
}