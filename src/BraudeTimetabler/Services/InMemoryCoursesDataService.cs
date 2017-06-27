using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Api;
using BraudeTimetabler.Models;

namespace BraudeTimetabler.Services
{
    public class InMemoryCoursesDataService : ICoursesDataService
    {
        private readonly ReadOnlyCollection<Course> courses;
        private readonly ReadOnlyCollection<CourseModel> coursesModels;

        public InMemoryCoursesDataService()
        {
            courses  = Algorithm.AllCoursesData.AsReadOnly();
            coursesModels = courses.Select(c => new CourseModel(c.Id, c.Name)).ToList().AsReadOnly();
        }

        public IReadOnlyCollection<CourseModel> GetAllModels() => coursesModels;

        public IReadOnlyCollection<Course> GetAll() => courses;
    }
}