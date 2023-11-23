using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.Repositories;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LanguageLearningSchool.Controllers
{
    public class LessonTaskController : Controller
    {
        private readonly ILessonTaskRepository _lessonTaskRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserAndLessonRepository _userAndLessonRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly UserManager<User> _userManager;
        private readonly IUserAndTaskRepository _userAndTaskRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAndCourseRepository _userAndCourseRepository;

        public LessonTaskController(IUserAndLessonRepository userAndLessonRepository, ILessonRepository lessonRepository,
            ILessonTaskRepository lessonTaskRepository, IAnswerRepository answerRepository, UserManager<User> userManager,
            IUserAndTaskRepository userAndTaskRepository, ICourseRepository courseRepository, IUserAndCourseRepository userAndCourseRepository)
        {
            _userAndLessonRepository = userAndLessonRepository;
            _lessonRepository = lessonRepository;
            _lessonTaskRepository = lessonTaskRepository;
            _answerRepository = answerRepository;
            _userManager = userManager;
            _userAndTaskRepository = userAndTaskRepository;
            _courseRepository = courseRepository;
            _userAndCourseRepository = userAndCourseRepository;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TestDetails(int lessonId)
        {
            var lesson = _lessonRepository.GetById(lessonId);
            var tasks = _lessonTaskRepository.GetByLessonId(lessonId);
            var answers = _answerRepository.GetAllAnswersOfLesson(lessonId);

            var testDetails = new TestDetailsViewModel
            {
                LessonId = lessonId,
                LessonName = lesson.LessonName,
                Tasks = tasks,
                Answers = answers
            };

            return View(testDetails);
        }

        public IActionResult Edit(int taskId)
        {
            var task = _lessonTaskRepository.GetById(taskId);
            var answers = _answerRepository.GetAllAnswersOfTask(taskId);

            var editTaskViewModel = new EditTaskViewModel
            {
                Task = task,
                Answers = answers
            };

            return View(editTaskViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditTaskViewModel taskViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit task");
                return View("Edit", taskViewModel);
            }

            var task = _lessonTaskRepository.GetById(taskViewModel.Task.TaskId);
            if (task != null)
            {
                task.TaskId = taskViewModel.Task.TaskId;
                task.LessonId = taskViewModel.Task.LessonId;
                task.TaskText = taskViewModel.Task.TaskText;

                _lessonTaskRepository.Update(task);

                foreach (var answer in taskViewModel.Answers)
                {
                    answer.TaskId = task.TaskId;
                    answer.Correctness = answer.AnswerName == taskViewModel.SelectedCorrectAnswer;
                }

                _answerRepository.UpdateRange(taskViewModel.Answers);

                var lessonId = _lessonTaskRepository.GetAll().Where(a => a.TaskId == taskViewModel.Task.TaskId)
                    .Select(a => a.LessonId).FirstOrDefault();
                var courseId = _lessonRepository.GetAll().Where(l => l.LessonId == lessonId).Select(l => l.CourseId)
                    .FirstOrDefault();

                return RedirectToAction("TestDetails", new
                {
                    lessonId = lessonId,
                    courseId = courseId
                });
            }
            else
            {
                return View(taskViewModel);
            }
        }

        [HttpGet]
        public IActionResult Create(int lessonId)
        {
            List<Answer> answers = new List<Answer>();

            for (int i = 0; i < 4; i++)
            {
                answers.Add(new Answer
                {
                    Correctness = false,
                    AnswerName = ""
                });
            }

            var createTaskViewModel = new CreateLessonTaskViewModel
            {
                LessonId = lessonId,
                Answers = answers
            };

            return View(createTaskViewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateLessonTaskViewModel createTaskViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to add task");
                return View("Create", createTaskViewModel);
            }

            var task = new LessonTask
            {
                LessonId = createTaskViewModel.LessonId,
                TaskText = createTaskViewModel.TaskText
            };
            _lessonTaskRepository.Add(task);

            if (task != null)
            {
                foreach (var answer in createTaskViewModel.Answers)
                {
                    answer.TaskId = task.TaskId;
                }

                _answerRepository.AddRange(createTaskViewModel.Answers);

                var lessonId = task.LessonId;
                var courseId = _lessonRepository.GetAll().Where(l => l.LessonId == lessonId).Select(l => l.CourseId)
                   .FirstOrDefault();

                return RedirectToAction("TestDetails", new
                {
                    lessonId = lessonId,
                    courseId = courseId
                });
            }
            else
            {
                return View(createTaskViewModel);
            }
        }

        [HttpGet]
        public IActionResult Delete(int taskId)
        {
            var task = _lessonTaskRepository.GetById(taskId);
            if (task == null) return View("Error");
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteTask(int taskId, int lessonId)
        {
            var task = _lessonTaskRepository.GetById(taskId);
            if (task == null) return View("Error");

            var answers = _answerRepository.GetAllAnswersOfTask(taskId);
            _answerRepository.DeleteRange(answers);

            var userAndTask = _userAndTaskRepository.GetAllUsersOnTask(taskId);
            _userAndTaskRepository.DeleteRange(userAndTask);

            _lessonTaskRepository.Delete(task);

            var courseId = _lessonRepository.GetAll().Where(l => l.LessonId == lessonId).Select(l => l.CourseId)
                .FirstOrDefault();

            return RedirectToAction("TestDetails", new
            {
                lessonId = lessonId,
                courseId = courseId
            });
        }

        public IActionResult PassTheTest(int lessonId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var lesson = _lessonRepository.GetById(lessonId);
            var tasks = _lessonTaskRepository.GetByLessonId(lessonId);
            var answers = _answerRepository.GetAllAnswersOfLesson(lessonId);

            var userAndTasks = new List<UserAndTask>();
            
            foreach(var task in tasks)
            {
                userAndTasks.Add(new UserAndTask
                {
                    UserAndTaskId = 0,
                    UserId = userId,
                    TaskId = task.TaskId,
                    UserAnswer = false
                });
            }

            var testDetails = new PassTheTestViewModel
                {
                    LessonId = lessonId,
                    LessonName = lesson.LessonName,
                    Tasks = tasks,
                    Answers = answers, 
                    UserAndTasks = userAndTasks
            };

            return View(testDetails);
        }

        [HttpPost]
        public IActionResult PassTheTest(PassTheTestViewModel passTheTestViewModel)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            const int MAXGRADE = 100;
            var tasksCount = passTheTestViewModel.Tasks.Count;
            var oneTaskEstimation = MAXGRADE / tasksCount;
            var lessonEstimation = 0;
            var numberOfCorrectAnswers = passTheTestViewModel.UserAndTasks.Where(answer => answer.UserAnswer == true).Count();
            if (numberOfCorrectAnswers == tasksCount)
            {
                lessonEstimation = 100;
            }
            else
            {
                lessonEstimation = numberOfCorrectAnswers * oneTaskEstimation;
            }

            var userAndLesson = _userAndLessonRepository.GetAll().Find(ul =>
            ul.UserId == userId && ul.LessonId == passTheTestViewModel.LessonId);
            userAndLesson.LessonEstimation = lessonEstimation;
            _userAndLessonRepository.Update(userAndLesson);

            var userAndTasks = new List<UserAndTask>();
            for(int i = 0; i < tasksCount; i++)
            {
                userAndTasks.Add(new UserAndTask
                {
                    UserId = userId,
                    TaskId = passTheTestViewModel.Tasks[i].TaskId,
                    UserAnswer = passTheTestViewModel.UserAndTasks[i].UserAnswer

                });
            }

            var existingUserAndTasks = _userAndTaskRepository.GetAll()
                .Where(ut => ut.UserId == userId && userAndTasks.Any(newUt => newUt.TaskId == ut.TaskId))
                .ToList();
            if (existingUserAndTasks.Any())
            {
                foreach (var existingUserAndTask in existingUserAndTasks)
                {
                    var newUserAndTask = userAndTasks.First(ut => ut.TaskId == existingUserAndTask.TaskId);
                    existingUserAndTask.UserAnswer = newUserAndTask.UserAnswer;
                    _userAndTaskRepository.Update(existingUserAndTask);
                }
            }
            else
            {
                _userAndTaskRepository.AddRange(userAndTasks);
            }

            var lesson = _lessonRepository.GetById(passTheTestViewModel.LessonId);
            var userAndCourse = _userAndCourseRepository.GetAll().Find(uc => uc.CourseId == lesson.CourseId &&
                uc.UserId == userId);

            var lessonsOnCourse = _lessonRepository.GetAll().Where(l => l.CourseId == lesson.CourseId).ToList();
            var userAndLessonList = _userAndLessonRepository
                .GetAll()
                .Join(_lessonRepository.GetAll(), ul => ul.LessonId, lesson => lesson.LessonId, (ul, lesson) => new { UserAndLesson = ul, Lesson = lesson })
                .Where(ul => ul.UserAndLesson.UserId == userId && ul.Lesson.CourseId == lesson.CourseId && ul.UserAndLesson.LessonEstimation > 60)
                .Select(ul => ul.UserAndLesson)
                .ToList();
            if (lessonsOnCourse.Count == userAndLessonList.Count)
            {
                userAndCourse.EndDate = DateTime.Now;
                int? allLessonsEstimation = 0;
                var numberOfLessons = lessonsOnCourse.Count;
                foreach (var lessonEstim in userAndLessonList)
                {
                    allLessonsEstimation += lessonEstim.LessonEstimation;
                }
                var generalEstimationOfCourse = allLessonsEstimation / numberOfLessons;
                userAndCourse.GeneralEstimation = generalEstimationOfCourse;
                _userAndCourseRepository.Update(userAndCourse);
            }

            return RedirectToAction("Detail", "Lesson", new
            {
                lessonId = lesson.LessonId,
                courseId = lesson.CourseId
            });
        }

    }
}
