namespace Api
{
    public abstract class ConstraintBase
    {
        protected ConstraintBase(bool isEnabled)
        {
            IsEnabled = isEnabled;
        }

        public abstract string Name
        {
            get;
        }
        public bool IsEnabled
        {
            get; set;
        }

        public abstract bool IsConstraintSatisfied(Timetable timetable);
    }
}