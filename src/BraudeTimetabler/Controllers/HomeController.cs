using System.Collections.Generic;
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
        private HomePageViewModel homePageViewModel;

        private HomePageViewModel HomePageViewModel
        {
        get
            {
                if (homePageViewModel == null)
                {
                    homePageViewModel = new HomePageViewModel();
                    // make async
                    homePageViewModel.AllCourses = coursesDataService.GetAllModels();
                }

                return homePageViewModel;
            }
        }

        public HomeController(ICoursesDataService coursesDataService)
        {
            this.coursesDataService = coursesDataService;
        }

        [HttpGet] 
        public IActionResult Index()
        {
            var model = HomePageViewModel;

            return View(model);
        }

        [HttpPost] // return from CourseDetails Page
        public IActionResult CourseDetails()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpGet] // this is get also (user has link and want a page in return)
        public IActionResult CourseDetails(string courseId)
        {
            var course = this.coursesDataService.Get(courseId);
            return View(course);
        }

        // Post - ReDirect - Get Pattern => To avoid forms re-submissions
        [HttpPost]
        public IActionResult GenerateTimetables(AlgorithmInputsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var scheduler = new Scheduler();
                var selectedCourses = new List<Course>();

                if (model.Ids == null)
                {
                    return Content(string.Empty);
                }

                foreach (var id in model.Ids)
                {
                    var course = coursesDataService.Get(id);
                    selectedCourses.Add(course);
                }

                var constraints = new ConstraintsCollection
                {
                    new MinimumFreeDaysConstraint(model.FreeDays),
                    new ClashesConstraint(model.Clashes),
                    new MaxGapBetweenClassesConstraint(model.MaxGap)
                };

                var solutions = scheduler.SolveSssp(selectedCourses, constraints);

                var response = solutions
                    .Select(s => s.ExportToJson())
                    .ToArray();

                return Json(response);

                // ReDirect To a HttpGet Action After a successful post operation.
                //return RedirectToAction(nameof(CourseDetails), new { courseId });
            }
            return null;
            // else, return the form with user previous entered values
            //return View(model);
        }
    }
}