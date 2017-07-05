namespace Api
{
    public abstract class ConstraintBase
    {
        protected ConstraintBase(bool calculateOnlyAfterTimetableIsReady)
        {
            CalculateOnlyAfterTimetableIsReady = calculateOnlyAfterTimetableIsReady;
        }

        public bool CalculateOnlyAfterTimetableIsReady
        {
            get;
        }

        public abstract string Name
        {
            get;
        }

        public abstract bool IsConstraintSatisfied(Timetable timetable);

        public abstract double GetRateFactor(Timetable timetable);

        public abstract double RatePenalty { get; }
    }
}