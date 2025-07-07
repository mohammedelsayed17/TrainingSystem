using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.ViewModels;
using TrainingSystem.Views.Account;

namespace TrainingSystem.Controllers
{
    // [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context)
        {
            _context = context;

            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            var departments = _context.Departments.ToList();
            var vm = new RegisterVM
            {
                Departments = departments
            };

            return View("Register", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await _userManager.FindByEmailAsync(model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "this email is already registered.");
                    model.Departments = _context.Departments.ToList();
                     return View("Register", model); // ضروري لإعادة

                }
                var user = new ApplicationUser
                {

                    Id = Guid.NewGuid().ToString(),
                    UserName = model.Email,
                    Email = model.Email
                };

                IdentityResult result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // Add to selected role
                    await _userManager.AddToRoleAsync(user, model.Role);
                    string fileName = null;
                    if (model.ImageFile != null)
                    {
                        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.ImageFile.FileName);
                        string filePath = Path.Combine(uploadsFolder, fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.ImageFile.CopyToAsync(stream);
                        }
                    }

                    // 👇 Add entry in Trainee or Instructor table
                    if (model.Role == "Trainee")
                    {
                        if (string.IsNullOrEmpty(fileName))
                        {
                            ModelState.AddModelError("ImageFile", "يجب رفع صورة للمدرب.");
                            model.Departments = _context.Departments.ToList(); // ضروري لإعادة تحميل الـ dropdown
                            return View("Register", model);
                        }
                        var trainee = new Trainee
                        {
                            Name = model.UserName,
                            UserId = user.Id,
                            Address = model.Address,
                            Grade = model.Grade,
                            DeptID = model.DeptID,
                            ImageUrl = fileName
                        };
                        _context.Trainees.Add(trainee);
                    }
                    else if (model.Role == "Instructor")
                    {
                        if (string.IsNullOrEmpty(fileName))
                        {
                            ModelState.AddModelError("ImageFile", "يجب رفع صورة للمدرب.");
                            model.Departments = _context.Departments.ToList(); // ضروري لإعادة تحميل الـ dropdown
                            return View("Register", model);
                        }
                        var instructor = new Instructor
                        {
                            Name = model.UserName,
                            UserId = user.Id,
                            Address = model.Address,
                            Deptid = model.DeptID,
                            Image = fileName
                        };
                        _context.Instructors.Add(instructor);
                    }

                    await _context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            model.Departments = _context.Departments.ToList(); // Re-populate departments for the view
            return View("Register", model);
        }



        public IActionResult Login()
        {
            return View("Login");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(model.UserName);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (found)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: model.RememberMe);

                        var roles = await _userManager.GetRolesAsync(user);
                        if (roles.Contains("Admin"))
                            return RedirectToAction("Index", "Dashboard");
                        else if (roles.Contains("Instructor"))
                            return RedirectToAction("Index", "Course");
                        else
                            return RedirectToAction("QR", "Trainee");
                    }

                }
                ModelState.AddModelError("", "invalid accpunt");

                // var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
                // if (result.Succeeded)
                // {
                //     return RedirectToAction("Index", "Home");
                // }
                // ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View("Login", model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult AccessDenied()
        {
            return View("AccessDenied");
        }



    }
}