using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.Repositories;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageLearningSchool.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAndCourseRepository _userAndCourseRepository;
        public LessonController(ILessonRepository lessonRepository, ICourseRepository courseRepository, 
            IUserAndCourseRepository userAndCourseRepository)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
            _userAndCourseRepository = userAndCourseRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int lessonId, int courseId)
        {
            var lesson = _lessonRepository.GetById(lessonId);
            var course = _courseRepository.GetById(courseId);

            var lessonInfo = new LessonDetailViewModel
            {
                Lesson = lesson,
                Course = course
            };

            return View(lessonInfo);
        }

        public IActionResult Create(int courseId)
        {
            var createModel = new CreateLessonViewModel
            {
                CourseId = courseId
            };
            return View(createModel);
        }

        [HttpPost]
        public IActionResult Create(CreateLessonViewModel lessonViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(lessonViewModel);
            }

            Lesson lesson = new Lesson
            {
                CourseId = lessonViewModel.CourseId,
                LessonName = lessonViewModel.LessonName,
                Material = lessonViewModel.Material
            };

            _lessonRepository.Add(lesson);

            var usersAndCourse = _userAndCourseRepository.GetAll().FindAll(item =>
                item.CourseId == lessonViewModel.CourseId).ToList();
            var lessons = _lessonRepository.GetAll().FindAll(item =>
                item.CourseId == lessonViewModel.CourseId).ToList();

            var course = _courseRepository.GetById(lessonViewModel.CourseId);

            var model = new CourseDetailsViewModel
            {
                Course = course,
                UsersAndCourse = usersAndCourse,
                Lessons = lessons
            };

            return RedirectToAction("Detail", "Course", new { id = lessonViewModel.CourseId });
        }

        public IActionResult Edit(int lessonId)
        {
            var lesson = _lessonRepository.GetById(lessonId);
            if (lesson == null) return View("Error");

            var lessonViewModel = new EditLessonViewModel
            {
                CourseId = lesson.CourseId,
                LessonId = lesson.LessonId,
                LessonName = lesson.LessonName,
                Material = lesson.Material
            };

            return View(lessonViewModel);
        }

        [HttpPost]
        public IActionResult Edit(int id, EditLessonViewModel lessonViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit lesson");
                return View("Edit", lessonViewModel);
            }

            var lesson = _lessonRepository.GetById(id);

            if (lesson != null)
            {
                lesson.LessonId = lessonViewModel.LessonId;
                lesson.CourseId = lessonViewModel.CourseId;
                lesson.LessonName = lessonViewModel.LessonName;
                lesson.Material = lessonViewModel.Material;

                _lessonRepository.Update(lesson);

                return RedirectToAction("Detail", new { lessonId = lessonViewModel.LessonId , 
                    courseId = lessonViewModel.CourseId});
            }
            else
            {
                return View(lessonViewModel);
            }

        }

        [HttpGet]
        public IActionResult Delete(int lessonId)
        {
            var lesson = _lessonRepository.GetById(lessonId);
            if (lesson == null) return View("Error");
            return View(lesson);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteLesson(int lessonId)
        {
            var lesson = _lessonRepository.GetById(lessonId);
            var courseId = lesson.CourseId;
            if (lesson == null) return View("Error");

            _lessonRepository.Delete(lesson);
            return RedirectToAction("Detail", "Course", new { id = courseId });
        }

    }
}
