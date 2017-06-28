using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Api
{
    public class CoursesFileParser
    {
        private string fileExtension;
        private const string SupportingExcelFileExtensions = ".xls;.xlsx;.xlt;.xlsm;.csv";

        public List<Course> Parse(string path)
        {
            this.Contract(path);

            // Read Excel file
            var dataSet = ExcelFileReader.ReadFile(path, fileExtension);

            // ExcelFile ===> courses :)
            var courses = ParseInternal(dataSet);

            return courses;
        }

        private List<Course> ParseInternal(DataSet dataSet)
        {
            var indexes = new ExcelCoursesFileColumnIndexes(dataSet.Tables[0].Columns);
            var dayOfWeekMap = CreateDayOfWeekMap();
            var rows = dataSet.Tables[0].Rows;
            var courseIdToRows = new Dictionary<string, List<ParsedCourseRow>>(rows.Count);

            foreach (DataRow row in rows)
            {
                var parsedRow = ParseExcelRow(row, indexes, dayOfWeekMap);

                List<ParsedCourseRow> courseGroups;
                if (courseIdToRows.TryGetValue(parsedRow.Id, out courseGroups))
                {
                    courseGroups.Add(parsedRow);
                }
                else
                {
                    courseGroups = new List<ParsedCourseRow>();
                    courseGroups.Add(parsedRow);

                    courseIdToRows.Add(parsedRow.Id, courseGroups);
                }
            }

            var courses = CreateCourses(courseIdToRows);
            return courses;
        }

        private List<Course> CreateCourses(Dictionary<string, List<ParsedCourseRow>> courseIdToRows)
        {
            var courses = new List<Course>(courseIdToRows.Keys.Count);

            foreach (var courseGroupsRows in courseIdToRows)
            {
                var courseId = courseGroupsRows.Key;

                var mainLecture = courseGroupsRows.Value.First();

                var mainLecturer = mainLecture.Lecturer;
                var courseName = mainLecture.GroupName;
                var academicPoints = mainLecture.AcademicPoints;

                var courseClassTypes = new List<ClassType>();

                // traverse course class-type
                foreach (var classType in courseGroupsRows.Value.GroupBy(g => g.ClassType))
                {
                    var classTypeName = classType.Key;

                    var courseGroups = new List<Group>();

                    // traverse class-type groups
                    foreach (var classTypeGroups in classType.GroupBy(x => string.IsNullOrEmpty(x.SubGroup) ? x.Group : $"{x.SubGroup}/{x.Group}"))
                    {
                        var courseGroupId = classTypeGroups.Key;

                        var courseGroupEvents = new List<GroupEvent>(3);
                        var lecturer = classTypeGroups.First().Lecturer;

                        // traverse class-type group events
                        foreach (var groupEvent in classTypeGroups)
                        {
                            var day = groupEvent.DayOfWeek;
                            var startHour = groupEvent.StartHour;
                            var endHour = groupEvent.EndHour;
                            var room = groupEvent.Room;

                            // we ignore groups who can't be scheduled 
                            if (day != null && startHour != null)
                            {
                                var courseGroupEvent = new GroupEvent((DayOfWeek)day, startHour, endHour, room);
                                courseGroupEvents.Add(courseGroupEvent);
                            }
                        }

                        if (courseGroupEvents.Any())
                        {
                            courseGroupEvents.Sort();
                            var courseGroup = new Group(courseGroupId, lecturer, courseGroupEvents.ToArray());
                            courseGroups.Add(courseGroup);
                        }
                        
                    }

                    if (courseGroups.Any())
                    {
                        var courseClassType = new ClassType(classTypeName, courseGroups);
                        courseClassTypes.Add(courseClassType);
                    }
                }

                if (courseClassTypes.Any())
                {
                    var course = new Course(courseId, courseName, mainLecturer, academicPoints, courseClassTypes);
                    courses.Add(course);
                }
            }

            PopulateReferences(courses);

            return courses;
        }

        private static void PopulateReferences(IEnumerable<Course> courses)
        {
            foreach (var course in courses)
            {
                foreach (var courseClassType in course.ClassTypes)
                {
                    courseClassType.Course = course;

                    foreach (var courseClassTypeGroup in courseClassType.Groups)
                    {
                        courseClassTypeGroup.ClassType = courseClassType;
                        courseClassTypeGroup.Course = course;

                        foreach (var groupEvent in courseClassTypeGroup.Events)
                        {
                            groupEvent.Group = courseClassTypeGroup;
                            groupEvent.ClassType = courseClassType;
                            groupEvent.Course = course;
                        }
                    }
                }
            }
        }

        private ParsedCourseRow ParseExcelRow(DataRow row, ExcelCoursesFileColumnIndexes indexes, IReadOnlyDictionary<string, DayOfWeek?> dayOfWeekMap)
        {
            var parsedRow = new ParsedCourseRow();

            parsedRow.Group = GetRowValue(row, indexes.Group);
            parsedRow.SubGroup = GetRowValue(row, indexes.SubGroup);
            parsedRow.Id = GetRowValue(row, indexes.Id);
            parsedRow.GroupName = GetRowValue(row, indexes.GroupName);

            parsedRow.ClassType = GetRowValue(row, indexes.ClassType);

            parsedRow.Lecturer = GetRowValue(row, indexes.Lecturer);

            var dayOfWeek = GetRowValue(row, indexes.DayOfWeek);
            parsedRow.DayOfWeek = dayOfWeekMap[dayOfWeek];

            var startHour = GetRowValue(row, indexes.StartHour);
            parsedRow.StartHour = TimeFactory.FromString(startHour);

            var endHour = GetRowValue(row, indexes.EndHour);
            parsedRow.EndHour = TimeFactory.FromString(endHour);

            parsedRow.Room = GetRowValue(row, indexes.Room);

            float academicPoints;
            float.TryParse(GetRowValue(row, indexes.AcademicPoints), out academicPoints);
            parsedRow.AcademicPoints = academicPoints;

            parsedRow.Faculty = GetRowValue(row, indexes.Faculty);

            return parsedRow;
        }

        private static Dictionary<string, DayOfWeek?> CreateDayOfWeekMap()
        {
            return new Dictionary<string, DayOfWeek?>
            {
                {"א", DayOfWeek.Sunday},
                {"ב", DayOfWeek.Monday},
                {"ג", DayOfWeek.Tuesday},
                {"ד", DayOfWeek.Wednesday},
                {"ה", DayOfWeek.Thursday},
                {"ו", DayOfWeek.Friday},
                {"ז", DayOfWeek.Saturday},
                {string.Empty, null}
            };
        }

        private string GetRowValue(DataRow row, int index)
        {
            var value = row.ItemArray[index].ToString().Trim();
            return string.Intern(value);
        }

        [DebuggerDisplay("{Id} {ClassType} {Lecturer}")]
        private class ParsedCourseRow
        {
            public string Group
            {
                get; set;
            }
            public string SubGroup
            {
                get; set;
            }
            public string Id
            {
                get; set;
            }
            public string GroupName
            {
                get; set;
            }
            public string ClassType
            {
                get; set;
            }
            public string Lecturer
            {
                get; set;
            }
            public DayOfWeek? DayOfWeek
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
            public float AcademicPoints
            {
                get; set;
            }
            public string Faculty
            {
                get; set;
            }

        }

        private struct ExcelCoursesFileColumnIndexes
        {
            private readonly DataColumnCollection columns;
            public int Group => GetIndex("קבוצה");
            public int SubGroup => GetIndex("הקבצה");
            public int Id => GetIndex("נושא");
            public int GroupName => GetIndex("תיאור נושא");
            public int ClassType => GetIndex("תיאור סוג מקצוע");
            public int Lecturer => GetIndex("שם מרצה");
            public int DayOfWeek => GetIndex("יום בשבוע");
            public int StartHour => GetIndex("שעת התחלה");
            public int EndHour => GetIndex("שעת סיום");
            public int Room => GetIndex("תיאור כיתה");
            public int AcademicPoints => GetIndex("נ\"ז");
            public int Faculty => GetIndex("תיאור חוג");

            public ExcelCoursesFileColumnIndexes(DataColumnCollection columns)
            {
                this.columns = columns;
            }

            private int GetIndex(string columnName)
            {
                return columns[columnName].Ordinal;
            }
        }

        private void Contract(string path)
        {
            // path not null
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path), "Courses file path is mandatory");
            }

            // file exists
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Courses file not exists in:{path}");
            }

            this.fileExtension = Path.GetExtension(path);

            // supported excel file extension
            if (!SupportingExcelFileExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("The Chosen file extension is invalid\n" +
                                            $"The supported Excel file extensions are: {SupportingExcelFileExtensions}.");
            }
        }
    }
}