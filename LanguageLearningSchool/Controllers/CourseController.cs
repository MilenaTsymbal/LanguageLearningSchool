using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Controllers
{
    public class CourseController : Controller
    {
        public CourseController(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public readonly ICourseRepository _courseRepository;

        public async Task<IActionResult> Index()
        {
            IEnumerable<Course>  courses = await _courseRepository.GetAll();
            return View(courses);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Course course = await _courseRepository.GetByIdAsync(id);
            return View(course);
        }
    }
}
