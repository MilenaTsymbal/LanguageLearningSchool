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
        public UserController(IUserRepository userRepository, UserManager<User> userManager, SignInManager<User> signInManager, IUserAndCourseRepository userAndCourseRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _userAndCourseRepository = userAndCourseRepository;
        }
        public readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
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
                //var courses = _userAndCourseRepository.GetAll().FindAll(item => item.UserId == user.Id).Select(item => item.Course).ToList();
                var userAndCourses = _userAndCourseRepository.GetAll().Where(item => item.UserId == user.Id).ToList();
                var model = new UserDetailViewModel
                {
                    User = user,
                    //Courses = userAndCourses.Select(item => item.Course).ToList(),
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
                var model = new UserDetailViewModel
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
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(CourseRegistrationViewModel viewModel)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var user = await _userManager.GetUserAsync(User);

        //        var isRegistered = _context.UserAndCourses
        //            .Any(uc => uc.CourseId == viewModel.CourseId && uc.UserId == user.Id);

        //        if (isRegistered)
        //        {
        //            TempData["Message"] = "You are already registered for this course.";
        //            return RedirectToAction("Details", new { id = viewModel.CourseId });
        //        }

        //        var userAndCourse = new UserAndCourse
        //        {
        //            UserId = user.Id,
        //            CourseId = viewModel.CourseId,
        //            StartDate = viewModel.StartDate,
        //            EndDate = viewModel.EndDate,
        //            GeneralEstimation = viewModel.GeneralEstimation,
        //        };

        //        _context.UserAndCourses.Add(userAndCourse);
        //        await _context.SaveChangesAsync();

        //        return RedirectToAction("Index", "Home");
        //    }

        //    return View(viewModel);
        //}
    }
}
