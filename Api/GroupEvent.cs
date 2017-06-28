using System;
using System.Diagnostics;

namespace Api
{
    [DebuggerDisplay("{Day} {StartHour}-{EndHour} {Room}")]
    public class GroupEvent : IComparable<GroupEvent>
    {
        public GroupEvent(DayOfWeek day, Time startHour, Time endHour, string room)
        {
            this.Day = day;
            this.StartHour = startHour;
            this.EndHour = endHour;
            this.Room = room;
        }

        public DayOfWeek Day
        {
            get; set;
        }
        public Time StartHour
        {
            get; set;
        }
        public Time EndHour
        {
            get; set;
        }
        public string Room
        {
            get; set;
        }

        public Group Group
        {
            get; set; 
        }

        public ClassType ClassType
        {
            get; set;
        }

        public Course Course
        {
            get; set;
        }

        public int CompareTo(GroupEvent other)
        {
            if (other == null)
            {
                return 1;
            }

            var returnValue = this.Day.CompareTo(other.Day);
            if (returnValue == 0)
            {
                // if same day, decide by start hour
                returnValue = this.StartHour.CompareTo(other.StartHour);
            }

            return returnValue;
        }

        public override string ToString()
        {
            return $"{Course}, {ClassType}, {Group}, {Room}";
        }
    }
}