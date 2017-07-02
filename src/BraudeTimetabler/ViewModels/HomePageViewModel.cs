using System.Collections.Generic;
using Api;

namespace BraudeTimetabler.Models
{
    public class HomePageViewModel
    {
        public IReadOnlyList<CourseViewModel> AllCourses { get; set; }
    }
}