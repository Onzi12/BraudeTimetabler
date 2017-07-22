using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Fitnesses;

namespace Api
{
    public class Timetable : IEnumerable<Group>, IComparer<Timetable>, IEqualityComparer<Timetable>, IComparable<Timetable>, IFitness
    {
        public TimeSlot[,] TimeSlotsTimetable
        {
            get;
        }

        public Timetable(IEnumerable<Group> groups = null, double rating = double.MaxValue)
        {
            if (groups == null)
            {
                groups = Enumerable.Empty<Group>();
            }

            TimeSlotsTimetable = new TimeSlot[Time.TotalHoursOfDay, Time.TotalSchoolDaysInWeek];
            courseGroups = new List<Group>();

            foreach (var group in groups)
            {
                Add(group);
            }

            CoursesGroups = courseGroups.AsReadOnly();
            this.Rating = rating;
        }

        public Timetable(Timetable instantiation, Group value) : this(instantiation.CoursesGroups)
        {
            this.Add(value);
        }

        public IReadOnlyList<Group> CoursesGroups { get; }

        public double Rating { get; private set; }

        public  double Rate(ConstraintsCollection constraints)
        {
            double timetableRate = 0;
            foreach (var constraint in constraints)
            {
                timetableRate += constraint.RatePenalty * constraint.GetRateFactor(this);
            }

            return Rating = timetableRate; // update Rating and return it
        }

        private readonly List<Group> courseGroups;

        public string ExportToJson()
        {
            var arr = new List<string[]>(Time.TotalHoursOfDay); 
            for (int i = 0; i < Time.TotalHoursOfDay; i++) 
            {
                var row = new string[Time.TotalSchoolDaysInWeek + 1];

                bool shouldAddTime = false;
                for (int j = 0; j < Time.TotalSchoolDaysInWeek; j++)
                {
                    var slotString = GetSlotToString(i, j);
                    shouldAddTime |= slotString != string.Empty;
                    row[j + 1] = GetSlotToString(i, j);
                }

                var timeString = string.Empty;
                if (shouldAddTime)
                {
                timeString = $"{Time.IndexHourToString(i)}-{Time.IndexHourToString(i, true)}";
                }
                row[0] = timeString;

                arr.Add(row);
            }

            return new JavaScriptSerializer().Serialize(new { timeslotsMatrix = arr, Rating});
        }

        private string GetSlotToString(int i, int j)
        {
            return TimeSlotsTimetable[i, j]?.ToString() ?? string.Empty;
        }

        public void Add(Group newGroup)
        {
            courseGroups.Add(newGroup);
            foreach (var groupEvent in newGroup.Events)
            {
                for (uint i = groupEvent.StartHour; i < groupEvent.EndHour; i++)
                {
                    var timeSlot = TimeSlotsTimetable[i, (int)groupEvent.Day];
                    if (timeSlot == null)
                    {
                        TimeSlotsTimetable[i, (int)groupEvent.Day] = new TimeSlot(groupEvent);
                    }
                    else
                        timeSlot.Events.Add(groupEvent);
                }
            }
        }

        public IEnumerator<Group> GetEnumerator()
        {
            return CoursesGroups.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Timetable Copy()
        {
            return new Timetable(this.CoursesGroups);
        }

        public int Compare(Timetable x, Timetable y) => x?.Rating.CompareTo(y?.Rating) ?? -1;
        public bool Equals(Timetable x, Timetable y) => x?.Rating.Equals(y?.Rating) ?? false;
        public int GetHashCode(Timetable obj) => obj.Rating.GetHashCode();
        public int CompareTo(Timetable other)
        {
            return Compare(this, other);
        }

        public double Evaluate(IChromosome chromosome)
        {
            throw new NotImplementedException();
        }
    }

    public class ClientTimetableRow
    {
        public string startHour;
        public string sunday;
        public string monday;
        public string tuesday;
        public string wednesday;
        public string thursday;
        public string friday;

    }
}