using System;

namespace Api
{
    public static class HelperMethods
    {
        /// <summary>
        /// stupid helper method, to make lazy read only prop easier
        /// </summary>
        /// <typeparam name="T">type of the field</typeparam>
        /// <param name="field">the field</param>
        /// <returns>the create field</returns>
        public static T GetOrCreate<T>(ref T field) where T : class, new()
        {
            return field ?? (field = new T());
        }

        public static string ToHebrew(this DayOfWeek day)
        {
            switch (day)
            {
                    case DayOfWeek.Sunday:
                        return "ראשון";
                    case DayOfWeek.Monday:
                        return "שני";
                    case DayOfWeek.Tuesday:
                        return "שלישי";
                    case DayOfWeek.Wednesday:
                        return "רביעי";
                    case DayOfWeek.Thursday:
                        return "חמישי";
                    case DayOfWeek.Friday:
                        return "שישי";
                    case DayOfWeek.Saturday:
                        return "שבת";
            }
            return "לא יום";
        }
    }
}