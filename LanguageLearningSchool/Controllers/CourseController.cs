﻿using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace LanguageLearningSchool.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        public readonly IUserRepository _userRepository;
        private readonly IUserAndCourseRepository _userAndCourseRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILessonRepository _lessonRepository;
        public CourseController(ICourseRepository courseRepository, IUserRepository userRepository, 
            IUserAndCourseRepository userAndCourseRepository, UserManager<User> userManager, 
            ILessonRepository lessonRepository)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _userAndCourseRepository = userAndCourseRepository;
            _userManager = userManager;
            _lessonRepository = lessonRepository;
        }
        
        public IActionResult Index()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = _userRepository.GetById(userId);

            if (user != null)
            {
                List<Course> courses = _courseRepository.GetAll();
                var userAndCourses = _userAndCourseRepository.GetAll().FindAll(item => item.UserId == user.Id).ToList();

                var coursesInfo = new CourseIndexViewModel
                {
                    Courses = courses,
                    UserAndCourses = userAndCourses
                };
                return View(coursesInfo);
            }

            return View("Error");
        }

        public IActionResult Detail(int id)
        {
            Course course = _courseRepository.GetById(id);
            var usersAndCourse = _userAndCourseRepository.GetAll().FindAll(item => item.CourseId == id).ToList();
            var lessons = _lessonRepository.GetAll().FindAll(item => item.CourseId == id).ToList();

            var model = new CourseDetailsViewModel
            {
                Course = course,
                UsersAndCourse = usersAndCourse,
                Lessons = lessons
            };
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (!ModelState.IsValid)
            {
                return View(course);
            }
            _courseRepository.Add(course);

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var dateOfCreation = DateTime.Now;

            UserAndCourse userAndCourse = new UserAndCourse
            {
                CourseId = course.CourseId,
                UserId = userId,
                DateOfCreation = dateOfCreation
            };

            _userAndCourseRepository.Add(userAndCourse);

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null) return View("Error");

            var courseViewModel = new EditCourseViewModel
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                Description = course.Description,
                Language = course.Language,
                DifficultyLevel = course.DifficultyLevel
            };
            return View(courseViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, EditCourseViewModel courseViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit course");
                return View("Edit", courseViewModel);
            }

            var course = _courseRepository.GetById(id);

            if (course != null)
            {
                course.CourseName = courseViewModel.CourseName;
                course.Description = courseViewModel.Description;
                course.Language = courseViewModel.Language;
                course.DifficultyLevel = courseViewModel.DifficultyLevel;

                _courseRepository.Update(course);

                return RedirectToAction("Index");
            }
            else
            {
                return View(courseViewModel);
            }

        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null) return View("Error");
            return View(course);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _courseRepository.GetById(id);
            if (course == null) return View("Error");

            _courseRepository.Delete(course);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddUserToCourse(int courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            DateTime startDate = DateTime.Now;

            UserAndCourse userAndCourse = new UserAndCourse
            {
                CourseId = courseId,
                UserId = userId,
                StartDate = startDate
            };

            _userAndCourseRepository.Add(userAndCourse);

            var user = _userRepository.GetById(userId);

            if (user != null)
            {
                List<Course> courses = _courseRepository.GetAll();
                var userAndCourses = _userAndCourseRepository.GetAll().FindAll(item => item.UserId == user.Id).ToList();

                var coursesInfo = new CourseIndexViewModel
                {
                    Courses = courses,
                    UserAndCourses = userAndCourses
                };
                return View("Index", coursesInfo);
            }

            return View("Error");
        }

        [HttpPost]
        public IActionResult RemoveUserFromCourse(int courseId, string userId)
        {
            var userAndCourse = _userAndCourseRepository.GetAll().Find(item => item.UserId == userId && item.CourseId == courseId);
            _userAndCourseRepository.Delete(userAndCourse);

            var user = _userRepository.GetById(userId);

            Course course = _courseRepository.GetById(courseId);
            var usersAndCourse = _userAndCourseRepository.GetAll().FindAll(item => item.CourseId == courseId).ToList();

            var model = new CourseDetailsViewModel
            {
                Course = course,
                UsersAndCourse = usersAndCourse
            };
            return View("Detail", model);

        }
    }
}
