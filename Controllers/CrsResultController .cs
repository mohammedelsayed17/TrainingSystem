using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.Services;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    // [Route("[controller]")]
    public class CrsResultController : Controller
    {
        private readonly ResultService service;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        public CrsResultController(ResultService service, UserManager<ApplicationUser> userManager, AppDbContext context)
        {
            _userManager = userManager;
            this.service = service;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var results = await service.GetAllAsync();
            return View("Index", results);
        }
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Create()
        {
            var vm = await service.PrepareVMAsync();
            return View("Create", vm);
        }
        [Authorize(Roles = "Admin,Instructor")]
        [HttpPost]
        public async Task<IActionResult> Create(CrsResultVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await service.PrepareVMAsync();
                return View("Create", vm);
            }

            await service.AddAsync(vm);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(int id)
        {
            var result = await service.GetByIdAsync(id);
            if (result == null) return NotFound();

            var vm = new CrsResultVM
            {
                Id = result.Id,
                Degree = result.Degree,
                CourseId = result.CourseId,
                TraineeId = result.TraineeId,
                Courses = await service.PrepareVMAsync().ContinueWith(t => t.Result.Courses),
                Trainees = await service.PrepareVMAsync().ContinueWith(t => t.Result.Trainees)
            };

            return View("Edit", vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Edit(CrsResultVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm = await service.PrepareVMAsync();
                return View("Edit", vm);
            }

            await service.UpdateAsync(vm);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin,Instructor")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> MyCourses()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // المستخدم مش موجود، ممكن ترجعه لصفحة تسجيل الدخول أو تعرض رسالة
                TempData["Error"] = "User not found. Please login again.";
                return RedirectToAction("Login", "Account");
            }

            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.UserId == user.Id);

            if (trainee == null)
            {
                // الحساب مفيهوش Trainee مرتبط (اتمسح أو فيه مشكلة)
                TempData["Error"] = "Trainee data not found. Please contact admin.";
                return RedirectToAction("Index", "Home"); // أو أي صفحة مناسبة
            }

            var results = await _context.crsResults
                .Where(r => r.TraineeId == trainee.Id)
                .Include(r => r.course)
                .Include(r => r.trainee)
                .ToListAsync();

            return View("MyCourses", results);
        }





    }
}