using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Controllers
{
    public class CourseController : Controller
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAndCourseRepository _userAndCourseRepository;
        public CourseController(ICourseRepository courseRepository, IUserAndCourseRepository userAndCourseRepository)
        {
            _courseRepository = courseRepository;
            _userAndCourseRepository = userAndCourseRepository;
        }
        
        public IActionResult Index()
        {
            List<Course>  courses = _courseRepository.GetAll();
            return View(courses);
        }

        public IActionResult Detail(int id)
        {
            Course course = _courseRepository.GetById(id);
            var users = _userAndCourseRepository.GetAll().FindAll(item => item.CourseId == id).Select(item => item.User).ToList();
            var model = new CourseDetailsViewModel
            {
                Course = course,
                Users = users
            };
            return View(model);
        }
    }
}
