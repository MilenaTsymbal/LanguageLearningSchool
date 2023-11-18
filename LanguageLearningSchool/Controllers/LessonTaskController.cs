using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.Repositories;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace LanguageLearningSchool.Controllers
{
    public class LessonTaskController : Controller
    {
        private readonly ILessonTaskRepository _lessonTaskRepository;
        private readonly ILessonRepository _lessonRepository;
        private readonly IUserAndLessonRepository _userAndLessonRepository;
        private readonly IAnswerRepository _answerRepository;

        public LessonTaskController(IUserAndLessonRepository userAndLessonRepository, ILessonRepository lessonRepository,
            ILessonTaskRepository lessonTaskRepository, IAnswerRepository answerRepository)
        {
            _userAndLessonRepository = userAndLessonRepository;
            _lessonRepository = lessonRepository;
            _lessonTaskRepository = lessonTaskRepository;
            _answerRepository = answerRepository;
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

        //[HttpPost]
        //public IActionResult Edit(EditTaskViewModel taskViewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        ModelState.AddModelError("", "Failed to edit task");
        //        return View("Edit", taskViewModel);
        //    }

        //    // 1. Сначала обновите связанные ответы
        //    _answerRepository.UpdateRange(taskViewModel.Answers);

        //    // 2. Затем обновите задание
        //    var task = _lessonTaskRepository.GetById(taskViewModel.Task.TaskId);
        //    if (task != null)
        //    {
        //        task.TaskId = taskViewModel.Task.TaskId;
        //        task.LessonId = taskViewModel.Task.LessonId;
        //        task.TaskText = taskViewModel.Task.TaskText;

        //        _lessonTaskRepository.Update(task);

        //        var lessonId = _lessonTaskRepository.GetAll().Where(a => a.TaskId == taskViewModel.Task.TaskId)
        //            .Select(a => a.LessonId).FirstOrDefault();
        //        var courseId = _lessonRepository.GetAll().Where(l => l.LessonId == lessonId).Select(l => l.CourseId)
        //            .FirstOrDefault();

        //        return RedirectToAction("TestDetails", new
        //        {
        //            lessonId = lessonId,
        //            courseId = courseId
        //        });
        //    }
        //    else
        //    {
        //        return View(taskViewModel);
        //    }
        //}
    }
}
