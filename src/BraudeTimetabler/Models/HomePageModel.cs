using System.Collections.Generic;
using Api;

namespace BraudeTimetabler.Models
{
    public class HomePageModel
    {
        private List<CourseModel> selectedCourses;
        public IReadOnlyList<CourseModel> AllCourses { get; set; }

        public List<CourseModel> SelectedCourses => HelperMethods.GetOrCreate(ref selectedCourses);
    }
}