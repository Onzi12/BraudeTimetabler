using System.Collections.Generic;
using System.Diagnostics;

namespace Api
{

    [DebuggerDisplay("{Id} {AcademicPoints} {Name}")]
        public class Course
    {
        public Course(string id, string name, string mainLecturer, float academicPoints, List<ClassType> classTypes = null)
        {
            Id = id;
            Name = name;
            MainLecturer = mainLecturer;
            AcademicPoints = academicPoints;
            if (classTypes == null)
            {
                classTypes = new List<ClassType>();
            }

            ClassTypes = classTypes;
        }
         public string Id { get; set; }

         public string Name { get; set; }

         public string MainLecturer { get; set; }

         public float AcademicPoints { get; set; }

        public List<ClassType> ClassTypes { get; set; }

        public override string ToString()
        {
            if (Name.Length < 10)
            {
                return this.Name;
            }
            return Name.Substring(0, 10) + "...";
        }
    }
}