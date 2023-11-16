using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace LanguageLearningSchool.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonRepository _lessonRepository;
        public LessonController(ILessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(int lessonId)
        {
            var lesson = _lessonRepository.GetById(lessonId);

            var lessonInfo = new LessonDetailViewModel
            {
                Lesson = lesson
            };

            return View(lessonInfo);
        }
    }
}
