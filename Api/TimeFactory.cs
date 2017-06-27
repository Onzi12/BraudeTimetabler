using System;
using System.Collections.Generic;

namespace Api
{
    public class TimeFactory
    {
        public static Time FromString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            if (str.Length > 5) // 5 is the length of hour:minutes. Example: 08:30
            {
                var indexOfParantesis = str.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                str = str.Substring(indexOfParantesis - 2, 5);
                //str = str.Substring(9, 5); // convert from 12/31/99 08:30:00 (braude format) To our desired format: 08:30
            }

            Time time;
            if (StringToTime.TryGetValue(str, out time))
            {
                return time;
            }

            return null;
        }

        static TimeFactory()
        {
            var totalHoursInaDay = Time.TotalHoursOfDay;
            var times = new Time[totalHoursInaDay + 1]; // 13:15 is only an end-time

            times[0] = new Time(8, 30, 1);
            times[1] = new Time(9, 30, 2);
            times[2] = new Time(10, 30, 3);
            times[3] = new Time(11, 30, 4);
            times[4] = new Time(12, 30, 5);
            times[5] = new Time(13, 15, 6);
            times[6] = new Time(13, 45, 6);
            times[7] = new Time(14, 40, 7);
            times[8] = new Time(15, 40, 8);
            times[9] = new Time(16, 40, 9);
            times[10] = new Time(17, 40, 10);
            times[11] = new Time(18, 40, 11);
            times[12] = new Time(19, 40, 12);
            times[13] = new Time(20, 40, 13);
            times[14] = new Time(21, 40, 14);
            times[15] = new Time(22, 40, 15);

            StringToTime = new Dictionary<string, Time>(totalHoursInaDay + 1);
            IndexToTime = new Dictionary<int, Time>(totalHoursInaDay);
            for (var index = 0; index < times.Length; index++)
            {
                var time = times[index];
                StringToTime.Add(time.ToString(), time);
                if (index == 5)
                {
                   continue; // ignore 13:15 
                }
                IndexToTime.Add((int)time.HourOfDay, time);
            }
        }

        public static Dictionary<int, Time> IndexToTime { get; set; }

        private static readonly Dictionary<string, Time> StringToTime;

        public static Time FromIndex(int i)
        {
            return IndexToTime[i];
        }
    }
}