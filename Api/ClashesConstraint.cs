namespace Api
{
    public class ClashesConstraint : ConstraintBase
    {
        public ClashesConstraint(bool clashesAllowed, bool isEnabled = true) : base(isEnabled)
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