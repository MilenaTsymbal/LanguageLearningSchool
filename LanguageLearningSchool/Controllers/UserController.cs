using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace LanguageLearningSchool.Controllers
{
    public class UserController : Controller
    {
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public readonly IUserRepository _userRepository;

        public async Task<IActionResult> Index()
        {
            IEnumerable<User> users = await _userRepository.GetAll();
            return View(users);
        }
    }
}
