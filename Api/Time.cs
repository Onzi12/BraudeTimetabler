using System;

namespace Api
{
    public class Time : IEquatable<Time>
    {
        public Time(uint hour, uint minute, uint hourOfDay)
        {
            Hour = hour;
            Minute = minute;
            this.HourOfDay = hourOfDay;
        }

        public uint Hour
        {
            get;
        }
        public uint Minute
        {
            get;
        }

        public uint HourOfDay
        {
            get;
        }

        public static int TotalHoursOfDay => 15;
        public static int TotalSchoolDaysInWeek => 6;

        #region Comparison operators And Equals
        public bool Equals(Time other)
        {
            return HourOfDay.Equals(other.HourOfDay);
        }

        public override int GetHashCode()
        {
            return HourOfDay.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || HourOfDay.GetType() != obj.GetType())
            {
                return false;
            }

            return Equals((Time)obj);
        }

        public static bool operator <(Time t1, Time t2)
        {
            return t1.HourOfDay < t2.HourOfDay;
        }

        public static bool operator >(Time t1, Time t2)
        {
            return t1.HourOfDay > t2.HourOfDay;
        }

        public static bool operator <=(Time t1, Time t2)
        {
            return t1.HourOfDay <= t2.HourOfDay;
        }

        public static bool operator >=(Time t1, Time t2)
        {
            return t1.HourOfDay >= t2.HourOfDay;
        }

        public static implicit operator uint(Time time)
        {
            return time.HourOfDay;
        }
        #endregion

        public override string ToString()
        {
            return $"{Hour:00}:{Minute:00}"; // time with leading zeros 
        }

        public static string IndexHourToString(int i)
        {
            return TimeFactory.FromIndex(i).ToString();
        }
    }
}