using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Api
{
    public class ConstraintsCollection : ReadOnlyCollection<ConstraintBase>
    {
        public bool IsConsistent(Timetable timetable, Group candidate)
        {
            timetable.Add(candidate);
            foreach (var constraintBase in Items)
            {
                if (constraintBase.IsEnabled && constraintBase.IsConstraintSatisfied(timetable) == false)
                {
                    return false;
                }
            }
            return true;
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