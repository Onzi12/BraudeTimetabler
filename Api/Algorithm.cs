using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace Api
{
    public class Algorithm
    {
        private static List<Course> realCoursesData;

        //to do here: implement the methods we need
        public static void TestAlgorithm()
        {
            // mock constraints
            var constraints = new ConstraintsCollection();
            constraints.Add(new MinimumFreeDaysConstraint(3));
            constraints.Add(new ClashesConstraint(true));

            // create courses

            var i = 0;
            var courses = AllCoursesData.TakeWhile(x => i++ < 6).ToList();

            var scheduler = new Scheduler();
            var allSolutions = scheduler.SolveSssp(courses, constraints);

            Console.WriteLine(allSolutions.Count);
            foreach (var solution in allSolutions)
            {
                solution.ExportToJason();
                if (constraints.IsConsistent(solution, solution.First()) == false)
                {
                    throw new Exception();
                }
                //PrintSolution(solution);
            }
        }

        public static List<Course> AllCoursesData => realCoursesData ?? (realCoursesData = CreateCoursesData());

        private static List<Course> CreateCoursesData()
        {
            var path = @"wwwroot\BraudeCoursesInfo.xlsx";
            var parser = new CoursesFileParser();

            var courses = parser.Parse(path);
            return courses;
        }

        private static void PrintSolution(Timetable solution)
        {
            var timeslotsTimetable = solution.TimeSlotsTimetable;
            for (int j = 0; j < Time.TotalHoursOfDay; j++)
            {
                for (int i = 0; i < Time.TotalSchoolDaysInWeek; i++)
                {
                    var slot = timeslotsTimetable[j, i];
                    if (slot == null)
                    {
                        Console.Write("-----\t");
                    }
                    else
                    {
                        var enumerable = slot.Events.Select(x => x.Room + "/");
                        var sb = new StringBuilder();
                        foreach (var s in enumerable)
                        {
                            sb.Append(s);
                        }
                        sb.Append('\t');
                        Console.Write(sb);
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
        }

    }
}