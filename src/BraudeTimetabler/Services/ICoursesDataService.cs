using System.Collections.Generic;
using Api;
using BraudeTimetabler.Models;

namespace BraudeTimetabler.Services
{
    public interface ICoursesDataService
    {
        IReadOnlyCollection<CourseModel> GetAllModels();
        IReadOnlyCollection<Course> GetAll();
    }
}