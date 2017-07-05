using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Api
{
    public class ConstraintsCollection : ReadOnlyCollection<ConstraintBase>
    {
        public bool IsConsistent(Timetable timetable, bool isFullInstantiation)
        {
            foreach (var constraintBase in Items)
            {
                if (constraintBase.CalculateOnlyAfterTimetableIsReady == isFullInstantiation && constraintBase.IsConstraintSatisfied(timetable) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsConsistent(Timetable timetable, Group candidate, bool isFullInstantiation = false)
        {
            timetable.Add(candidate);
            return IsConsistent(timetable, isFullInstantiation);
        }

        public ConstraintsCollection()
            : base(new List<ConstraintBase>())
        {
        }

        public void Add(ConstraintBase constraint)
        {
            this.Items.Add(constraint);
        }
    }
}