using LanguageLearningSchool.Enums;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.Repositories;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageLearningSchool.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAndCourseRepository _userAndCourseRepository;
        private readonly IUserAndLessonRepository _userAndLessonRepository;
        private readonly ILessonTaskRepository _lessonTaskRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserAndTaskRepository _userAndTaskRepository;
        public readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;

        public LessonController(ILessonRepository lessonRepository, ICourseRepository courseRepository, 
            IUserAndCourseRepository userAndCourseRepository, IUserAndLessonRepository userAndLessonRepository,
            ILessonTaskRepository lessonTaskRepository, IAnswerRepository answerRepository, 
            IUserAndTaskRepository userAndTaskRepository, IUserRepository userRepository, UserManager<User> userManager)
        {
            _lessonRepository = lessonRepository;
            _courseRepository = courseRepository;
            _userAndCourseRepository = userAndCourseRepository;
            _userAndLessonRepository = userAndLessonRepository;
            _lessonTaskRepository = lessonTaskRepository;
            _answerRepository = answerRepository;            _userAndTaskRepository = userAndTaskRepository;
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int lessonId, int courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = _userRepository.GetAll().Find(u => u.Id == userId);

            var lesson = _lessonRepository.GetById(lessonId);
            var userIsLearning = _userAndLessonRepository.GetAll()
                .FirstOrDefault(item => item.UserId == userId && item.LessonId == lessonId);
            var tasks = _lessonTaskRepository.GetAll().Where(lt => lt.LessonId == lessonId).ToList();
            if(user.UserRole != Roles.Teacher)
            {
                if (userIsLearning == null)
                {
                    var userAndLesson = new UserAndLesson
                    {
                        UserId = userId,
                        LessonId = lessonId
                    };
                    _userAndLessonRepository.Add(userAndLesson);
                }
            }
            
            userIsLearning = _userAndLessonRepository.GetAll()
                .FirstOrDefault(item => item.UserId == userId && item.LessonId == lessonId);
            var course = _courseRepository.GetById(courseId);
            var usersAndCourse = _userAndCourseRepository.GetAll().FindAll(item => item.CourseId == courseId).ToList();

            var lessonInfo = new LessonDetailViewModel
            {
                Lesson = lesson,
                Course = course,
                UsersAndCourse = usersAndCourse,
                UserAndLesson = userIsLearning,
                Tasks = tasks
            };

            return View(lessonInfo);
        }

        public IActionResult Create(int courseId)
        {
            var course = _courseRepository.GetById(courseId);
            var createModel = new CreateLessonViewModel
            {
                CourseId = courseId,
                CourseName = course.CourseName
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
                Material = lessonViewModel.Material,
                IsTheLastLesson = lessonViewModel.IsTheLastLesson
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
                Material = lesson.Material,
                IsTheLastLesson = lesson.IsTheLastLesson
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
                lesson.IsTheLastLesson = lessonViewModel.IsTheLastLesson;

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
            var courseId = lesson?.CourseId; // Use null conditional operator to handle lesson being null
            if (lesson == null) return View("Error");

            var allAnswersForLesson = _answerRepository.GetAllAnswersOfLesson(lessonId);

            // Use null conditional operator to handle potential null Task properties
            var userAndTasksForLesson = _userAndTaskRepository.GetAll()
                .Where(userAndTask => userAndTask.Task?.LessonId == lessonId).ToList();

            var lessonTasks = _lessonTaskRepository.GetAll().Where(lt => lt.LessonId == lessonId).ToList();
            var userAndLessons = _userAndLessonRepository.GetAll().Where(ul => ul.LessonId == lessonId).ToList();

            if (allAnswersForLesson != null && allAnswersForLesson.Any())
            {
                _answerRepository.DeleteRange(allAnswersForLesson);
            }
            if (userAndTasksForLesson != null && userAndTasksForLesson.Any())
            {
                _userAndTaskRepository.DeleteRange(userAndTasksForLesson);
            }
            if (lessonTasks != null && lessonTasks.Any())
            {
                _lessonTaskRepository.DeleteRange(lessonTasks);
            }
            if (userAndLessons != null && userAndLessons.Any())
            {
                _userAndLessonRepository.DeleteRange(userAndLessons);
            }

            _lessonRepository.Delete(lesson);

            return RedirectToAction("Detail", "Course", new { id = courseId });
        }

    }
}
