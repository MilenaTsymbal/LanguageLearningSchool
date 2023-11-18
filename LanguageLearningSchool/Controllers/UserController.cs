using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.Repositories;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Security.Claims;

namespace LanguageLearningSchool.Controllers
{
    public class UserController : Controller
    {
        public UserController(IUserRepository userRepository, UserManager<User> userManager,
            SignInManager<User> signInManager, ICourseRepository courseRepository, 
            IUserAndCourseRepository userAndCourseRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _courseRepository = courseRepository;
            _userAndCourseRepository = userAndCourseRepository;
        }
        public readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserAndCourseRepository _userAndCourseRepository;

        public IActionResult Index()
        {
            List<User> users = _userRepository.GetAll();
            return View(users);
        }

        public IActionResult UserDetailAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            var user = _userRepository.GetById(userId);
            
            if (user != null)
            {
                var userAndCourses = _userAndCourseRepository.GetAll().Where(item => item.UserId == user.Id).ToList();
                var model = new UserDetailsViewModel
                {
                    User = user,
                    UserAndCourses = userAndCourses
                };

                return View(model);
            }
            return View("Error");
        }

        public IActionResult Detail(string id)
        {
            var user = _userRepository.GetById(id);
            
            if (user != null)
            {
                var userAndCourses = _userAndCourseRepository.GetAll().Where(item => item.UserId == user.Id).ToList();
                var model = new UserDetailsViewModel
                {
                    User = user,
                    UserAndCourses = userAndCourses
                };

                return View(model);
            }
            return View("Error");
        }

        public async Task<IActionResult> EditAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return View("Error");
            var userViewModel = new EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                MiddleName = user.MiddleName,
                Surname = user.Surname,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                UserRole = user.UserRole
            };
            return View(userViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditAsync(EditUserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit course");
                return View("EditAsync", userViewModel);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                user.Id = userViewModel.Id;
                user.Name = userViewModel.Name;
                user.MiddleName = userViewModel.MiddleName;
                user.Surname = userViewModel.Surname;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.Email = userViewModel.Email;
                user.UserRole = userViewModel.UserRole;

                _userRepository.Update(user);

                return RedirectToAction("UserDetail");
            }
            else
            {
                return View(userViewModel);
            }

        }

        [HttpGet]
        public async Task<IActionResult> Delete()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return View("Error");

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteUser()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return View("Error");

            _userRepository.Delete(user);

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult LeaveCourse(int courseId)
        {
            var course = _courseRepository.GetById(courseId);

            var userLeaveCourseModel = new LeaveCourseViewModel
            {
                Course = course
            };

            return View(userLeaveCourseModel);
        }

        [HttpPost]
        public IActionResult ConfirmLeaveCourse(int courseId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            var userAndCourse = _userAndCourseRepository.GetAll().Find(item => item.UserId == userId && item.CourseId == courseId);

            _userAndCourseRepository.Delete(userAndCourse);


            var user = _userRepository.GetById(userId);

            if (user != null)
            {
                var userAndCourses = _userAndCourseRepository.GetAll().Where(item => item.UserId == user.Id).ToList();
                var model = new UserDetailsViewModel
                {
                    User = user,
                    UserAndCourses = userAndCourses
                };

                return View("UserDetail", model);
            }

            return View("Error");
        }
        //public IActionResult LeaveCourse(int courseId)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        //    return View();
        //}
        //[HttpPost, ActionName("LeaveCourse")]
        //public IActionResult LeaveCourse(int courseId)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

        //    var userAndCourse = _userAndCourseRepository.GetAll().Find(item => item.UserId == userId && item.CourseId == courseId);
        //    _userAndCourseRepository.Delete(userAndCourse);

        //    var user = _userRepository.GetById(userId);

        //    if (user != null)
        //    {
        //        var userAndCourses = _userAndCourseRepository.GetAll().Where(item => item.UserId == user.Id).ToList();
        //        var model = new UserDetailViewModel
        //        {
        //            User = user,
        //            UserAndCourses = userAndCourses
        //        };

        //        return View("UserDeatil", model);
        //    }

        //    return View("Error");
        //}
    }
}
