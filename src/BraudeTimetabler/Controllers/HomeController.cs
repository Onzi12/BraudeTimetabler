using System;
using System.Linq;
using Api;
using BraudeTimetabler.Models;
using BraudeTimetabler.Services;
using Microsoft.AspNetCore.Mvc;

namespace BraudeTimetabler.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoursesDataService coursesDataService;

        public HomeController(ICoursesDataService coursesDataService)
        {
            this.coursesDataService = coursesDataService;
        }

        public IActionResult Index()
        {
            var model = new HomePageModel();

            try
            {
                model.AllCourses = coursesDataService.GetAllModels();
            }
            catch (Exception e)
            {
                return Content(e.ToString());
            }

            model.SelectedCourses.Add(model.AllCourses[3]);
            return View(model);
        }

        public IActionResult CourseDetails(string courseId)
        {
            var course = this.coursesDataService.Get(courseId);
            return View(course);
        }
    }
}