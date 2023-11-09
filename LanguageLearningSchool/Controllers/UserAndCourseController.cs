using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Controllers
{
    public class UserAndCourseController : Controller
    {
        public UserAndCourseController(IUserAndCourseRepository userAndCourseRepository)
        {
            _userAndCourseRepository = userAndCourseRepository;
        }

        public readonly IUserAndCourseRepository _userAndCourseRepository;

        public IActionResult Index()
        {
            List<UserAndCourse> usersAndCourses = _userAndCourseRepository.GetAll();
            return View(usersAndCourses);
        }

        public IActionResult Detail(int id)
        {
            var UserAndCourse = _userAndCourseRepository.GetById(id);
            return View(UserAndCourse);
        }
    }
}
