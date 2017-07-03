using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Api
{
    public class Timetable : IEnumerable<Group>, IComparer<Timetable>, IEqualityComparer<Timetable>, IComparable<Timetable>
    {
        public TimeSlot[,] TimeSlotsTimetable
        {
            get;
        }

        public Timetable(IEnumerable<Group> groups = null)
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

        public string ExportToJason()
        {
            var arr = new ClientTimetableRow[Time.TotalHoursOfDay - 1];
            for (int i = 0; i < Time.TotalHoursOfDay - 1; i++) // 22:40 will always be empty
            {
                var j = 0;
                var row = new ClientTimetableRow();
                row.startHour = Time.IndexHourToString(i);
                row.sunday = GetSlotToString(i, j++);;
                row.monday = GetSlotToString(i, j++);
                row.tuesday = GetSlotToString(i, j++);
                row.wednesday = GetSlotToString(i, j++);
                row.thursday = GetSlotToString(i, j++);
                row.friday = GetSlotToString(i, j++);
                arr[i] = row;
            }

            var jason = new JavaScriptSerializer().Serialize(arr);
            Console.WriteLine(jason);
            
            return jason;
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