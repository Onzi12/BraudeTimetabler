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
            var times = new Time[totalHoursInaDay + 2]; // 13:15 && 22:40 are only an end-time
            times[0] = new Time(8, 30, 0);
            times[1] = new Time(9, 30, 1);
            times[2] = new Time(10, 30, 2);
            times[3] = new Time(11, 30, 3);
            times[4] = new Time(12, 30, 4);
            times[5] = new Time(13, 15, 5);
            times[6] = new Time(13, 45, 5);
            times[7] = new Time(14, 40, 6);
            times[8] = new Time(15, 40, 7);
            times[9] = new Time(16, 40, 8);
            times[10] = new Time(17, 40, 9);
            times[11] = new Time(18, 40, 10);
            times[12] = new Time(19, 40, 11);
            times[13] = new Time(20, 40, 12);
            times[14] = new Time(21, 40, 13);
            times[15] = new Time(22, 40, 14);

            var endTimes = new Time[totalHoursInaDay];
            endTimes[0] = new Time(9, 20, 0);
            endTimes[1] = new Time(10, 20, 1);
            endTimes[2] = new Time(11, 20, 2);
            endTimes[3] = new Time(12, 20, 3);
            endTimes[4] = new Time(13, 15, 4);
            endTimes[5] = new Time(14, 30, 5);
            endTimes[6] = new Time(15, 30, 6);
            endTimes[7] = new Time(16, 30, 7);
            endTimes[8] = new Time(17, 30, 8);
            endTimes[9] = new Time(18, 30, 9);
            endTimes[10] = new Time(19, 30, 10);
            endTimes[11] = new Time(20, 30, 11);
            endTimes[12] = new Time(21, 30, 12);
            endTimes[13] = new Time(22, 40, 13);

            StringToTime = new Dictionary<string, Time>(totalHoursInaDay + 1);
            IndexToTime = new Dictionary<int, Time>(totalHoursInaDay);
            EndIndexToTime = new Dictionary<int, Time>(totalHoursInaDay);
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

            foreach (var endTime in endTimes)
            {
                EndIndexToTime.Add((int)endTime.HourOfDay, endTime);
            }
        }

        public static Dictionary<int, Time> EndIndexToTime { get; set; }

        public static Dictionary<int, Time> IndexToTime { get; set; }

        private static readonly Dictionary<string, Time> StringToTime;

        public static Time FromIndex(int i, bool getEndTime)
        {
            return getEndTime ? EndIndexToTime[i] : IndexToTime[i];
        }
    }
}