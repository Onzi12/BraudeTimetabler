using System.Collections.Generic;
using Api;
using BraudeTimetabler.Models;

namespace BraudeTimetabler.Services
{
    public interface ICoursesDataService
    {
        IReadOnlyList<CourseViewModel> GetAllModels();
        IReadOnlyList<Course> GetAll();

        CourseViewModel GetModel(string id);

        Course Get(string id);
    }
}