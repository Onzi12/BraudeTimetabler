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
        private static IList<Course> realCoursesData;

        //to do here: implement the methods we need
        public static void TestAlgorithm()
        {
            // mock constraints
            var constraints = new ConstraintsCollection();
            constraints.Add(new MinimumFreeDaysConstraint(3));
            constraints.Add(new ClashesConstraint(true));

            // mock courses
            //var courses = MockCourses();
            //var courses = MockCoursesAsInBook();
            var i = 0;
            var courses = RealCoursesData.TakeWhile(x => i++ < 6).ToList();

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

        public static IList<Course> RealCoursesData => realCoursesData ?? (realCoursesData = CreateCoursesData());

        private static IList<Course> CreateCoursesData()
        {
            var path = $"wwwroot\\BraudeCoursesInfo.xlsx";
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

        //public static IList<Course> MockCoursesAsInBook()
        //{
        //    var calculus = new Course("1", "Calculus1", "Ronny Sivan", 3.5);
        //    var cLanguage = new Course("2", "C Language", "Elena Rave", 4);
        //    var g0Event = new GroupEvent(DayOfWeek.Wednesday, 11, 13, "g0");
        //    var g0 = new Group("g0", "g0", g0Event);

        //    var g1Event = new GroupEvent(DayOfWeek.Thursday, 11, 13, "g1");
        //    var g1 = new Group("g1", "g1", g1Event);

        //    var g2Event = new GroupEvent(DayOfWeek.Sunday, 9, 11, "g2");
        //    var g2 = new Group("g2", "g2", g2Event);

        //    var g3Event = new GroupEvent(DayOfWeek.Monday, 15, 17, "g3");
        //    var g3 = new Group("g3", "g3", g3Event);

        //    var calculusLectureGroups = new List<Group> { g0, g1, g2, g3 };
        //    var calculusLectures = new ClassType(ClassTypes.Lecture, calculusLectureGroups);
        //    calculus.ClassTypes.Add(calculusLectures);

        //    var g4Event = new GroupEvent(DayOfWeek.Sunday, 11, 14, "g4");
        //    var g4 = new Group("g4", "g4", g4Event);

        //    var calculusTutorialGroups = new List<Group> { g4 };
        //    var calculusTutorials = new ClassType(ClassTypes.Tutorial, calculusTutorialGroups);
        //    calculus.ClassTypes.Add(calculusTutorials);

        //    var g5Event = new GroupEvent(DayOfWeek.Sunday, 14, 16, "g5");
        //    var g5 = new Group("g5", "g5", g5Event);

        //    var g6Event = new GroupEvent(DayOfWeek.Monday, 17, 19, "g6");
        //    var g6 = new Group("g6", "g6", g6Event);

        //    var cLanguageLectureGroups = new List<Group> { g5, g6 };
        //    var cLanguageLectures = new ClassType(ClassTypes.Lecture, cLanguageLectureGroups);
        //    cLanguage.ClassTypes.Add(cLanguageLectures);

        //    var g7Event = new GroupEvent(DayOfWeek.Sunday, 9, 10, "g7");
        //    var g7 = new Group("g7", "g7", g7Event);

        //    var cLanguageTutorialGroups = new List<Group> { g7 };
        //    var cLanguageTutorials = new ClassType(ClassTypes.Tutorial, cLanguageTutorialGroups);
        //    cLanguage.ClassTypes.Add(cLanguageTutorials);

        //    var g8Event = new GroupEvent(DayOfWeek.Wednesday, 9, 11, "g8");
        //    var g8 = new Group("g8", "g8", g8Event);

        //    var cLanguageLabGroups = new List<Group> { g8 };
        //    var cLanguageLabs = new ClassType(ClassTypes.Lab, cLanguageLabGroups);
        //    cLanguage.ClassTypes.Add(cLanguageLabs);

        //    //var g9Event = new GroupEvent(DayOfWeek.Monday, 17, 19, "g9");
        //    //var g9 = new Group("g9", "g9", g9Event);

        //    return new List<Course>(2) { calculus, cLanguage };
        //}
        //private static IList<Course> MockCourses()
        //{
        //    var courses = new List<Course>(8);
        //    var classTypesNames = new string[]
        //    {
        //        ClassTypes.Lecture,
        //        ClassTypes.Lab,
        //        ClassTypes.Tutorial
        //    };

        //    for (uint i = 0; i < 8; i++)
        //    {

        //        var classTypes = new List<ClassType>();
        //        for (int j = 0; j < 3; j++)
        //        {
        //            var ij = i * j;
        //            var groups = new List<Group>();
        //            for (uint k = 0; k < 3; k++)
        //            {
        //                var ijk = (uint)ij * k;
        //                var mockEvent = new GroupEvent((DayOfWeek)(ijk % 7), (uint)ijk % 12, ijk % 12 + 1, "room");
        //                var group = new Group((i * k).ToString(), (i * k).ToString(), new[] { mockEvent });
        //                groups.Add(group);
        //            }
        //            var classType = new ClassType(classTypesNames[ij % 3], groups);
        //            classTypes.Add(classType);
        //        }
        //        var course = new Course(i.ToString(), i.ToString(), i.ToString(), i, classTypes);
        //        courses.Add(course);
        //    }

        //    return courses;
        //}
    }
}