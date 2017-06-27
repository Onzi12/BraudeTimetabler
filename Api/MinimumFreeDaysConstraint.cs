using System.Linq;

namespace Api
{
    public class MinimumFreeDaysConstraint : ConstraintBase
    {
        public MinimumFreeDaysConstraint(int minFreeDays, bool isEnabled = true) : base(isEnabled)
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

        private int NumberOfSchoolDays(Timetable timetable)
        {
            return timetable.SelectMany(t => t.Events).Select(e => e.Day).Distinct().Count();
        }
    }
}