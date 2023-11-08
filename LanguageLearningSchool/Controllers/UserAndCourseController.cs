using LanguageLearningSchool.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Controllers
{
    public class UserAndCourseController : Controller
    {
        public UserAndCourseController(ApplicationDbContext context)
        {
            _context = context;
        }
        public readonly ApplicationDbContext _context;


        public IActionResult Detail(int id)
        {
            var UserAndCourse = _context.UsersAndCourses.FirstOrDefault(c => c.CourseId == id);
            return View(UserAndCourse);
        }
    }
}
