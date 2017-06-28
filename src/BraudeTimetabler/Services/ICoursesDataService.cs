using System.Collections.Generic;
using Api;
using BraudeTimetabler.Models;

namespace BraudeTimetabler.Services
{
    public interface ICoursesDataService
    {
        IReadOnlyList<CourseModel> GetAllModels();
        IReadOnlyList<Course> GetAll();

        CourseModel GetModel(string id);

        Course Get(string id);
    }
}