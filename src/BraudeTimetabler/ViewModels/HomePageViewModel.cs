using System.Collections.Generic;
using Api;

namespace BraudeTimetabler.Models
{
    public class HomePageViewModel
    {
        private List<CourseViewModel> selectedCourses;
        public IReadOnlyList<CourseViewModel> AllCourses { get; set; }

        public List<CourseViewModel> SelectedCourses => HelperMethods.GetOrCreate(ref selectedCourses);
    }
}