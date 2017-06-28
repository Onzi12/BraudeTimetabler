using System;
using System.Linq;
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

        [HttpGet] // when user wants to get a page
                  // FurtherMore: 
                  // Get is for user read operations. 
                  // Post is for user write operations. 
        public IActionResult Index()
        {
            var model = HomePageViewModel;

            if (!model.SelectedCourses.Any())
            {
                model.SelectedCourses.Add(model.AllCourses[3]);
                model.SelectedCourses.Add(model.AllCourses[22]);
            }
            
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
        [ValidateAntiForgeryToken] // validate that the form arrived after a valid get request
        public IActionResult PostReDirectGetExample(string courseId)
        {
            if (ModelState.IsValid)
            {
                // do things with data...

                // ReDirect To a HttpGet Action After a successful post operation.
                return RedirectToAction(nameof(CourseDetails), new { courseId });
            }

            // else, return the form with user previous entered values
            return View();
        }
    }
}