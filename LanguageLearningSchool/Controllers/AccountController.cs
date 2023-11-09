using LanguageLearningSchool.Enums;
using LanguageLearningSchool.Models;
using LanguageLearningSchool.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LanguageLearningSchool.Controllers;

public class AccountController : Controller
{
    private readonly UserManager<User>   _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    
    // GET
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    //POST
    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid)
        {
            // Model state is not valid
            return View(loginViewModel);
        }

        var user = await _userManager.FindByNameAsync(loginViewModel.Email);

        if (user != null)
        {
            // User is found. Password checking
            var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);

            if (passwordCheck)
            {
                // Password is correct. Trying to sign in
                var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);

                if (result.Succeeded)
                {
                    // User has successfully signed in
                    return RedirectToAction("Index", "Home");
                }
            }
             
            // Password is incorrect
            TempData["Error"] = "Wrong password";
            return View(loginViewModel);
        }

        //user not found
        TempData["Error"] = "Wrong credentials";
        
        return RedirectToAction("Register");
    }
    
    // GET
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    //POST
    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(registerViewModel);
        }

        var user = await _userManager.FindByEmailAsync(registerViewModel.Email);
        
        // If we have a user
        if (user != null)
        {
            TempData["Error"] = "This email address is already in use";

            return View(registerViewModel);
        }

        var newUser = new User
        {
            Name = registerViewModel.Name,
            MiddleName = registerViewModel.MiddleName,
            Surname = registerViewModel.Surname,
            PhoneNumber = registerViewModel.PhoneNumber,
            UserName = registerViewModel.Email,
            Email = registerViewModel.Email,
            UserRole = Roles.Student
        };
        var newUserResponse = await _userManager.CreateAsync(newUser, registerViewModel.Password);

        if (!await _roleManager.RoleExistsAsync(Roles.Student.ToString()))
        {
            await _roleManager.CreateAsync(new IdentityRole(Roles.Student.ToString()));
        }

        var roleResult = await _userManager.AddToRoleAsync(newUser, Roles.Student.ToString());


        return RedirectToAction("Index", "Home");
        
    }

    [HttpGet]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();

        return RedirectToAction("Index", "Home");
    }
}