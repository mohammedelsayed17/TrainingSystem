using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRCoder;
using TrainingSystem.Services;
using TrainingSystem.ViewModels;
using TrainingSystem.Helpers;
using QRCodeHelper = TrainingSystem.Helpers.QRCodeHelper;
using TrainingSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TrainingSystem.Models;

namespace TrainingSystem.Controllers
{
    // [Route("[controller]")]
    // [Authorize(Roles = "Admin")]
    public class TraineeController : Controller
    {
        private readonly TraineeService service;
        private readonly PdfService _pdfService;
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public TraineeController(TraineeService service, PdfService pdfService, AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            this.service = service;
            _pdfService = pdfService;
            _context = context;
            _userManager = userManager;
        }
        // [Authorize(Roles = "Admin")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var trainees = await service.GetAllAsync();
            return View("Index", trainees);
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var vm = await service.PrepareVMAsync();
            return View("Create", vm);
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(TraineeVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = (await service.PrepareVMAsync()).Departments;
                return View("Create", vm);
            }

            await service.AddAsync(vm);
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var trainee = await service.GetByIdAsync(id);
            if (trainee == null) return NotFound();

            var vm = new TraineeVM
            {
                Id = trainee.Id,
                Name = trainee.Name,
                Address = trainee.Address,
                Grade = trainee.Grade,
                DeptID = trainee.DeptID,
                ImageUrl = trainee.ImageUrl,
                Departments = (await service.PrepareVMAsync()).Departments
            };

            return View("Edit", vm);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(TraineeVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = (await service.PrepareVMAsync()).Departments;
                return View("Edit", vm);
            }

            await service.UpdateAsync(vm);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // [Authorize(Roles = "Admin")]
        [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> QRById(int id)
        {
            var userId = _userManager.GetUserId(User);
            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (trainee == null)
                return NotFound();

            string qrText = $"Name: {trainee.Name}, ID: {trainee.Id}, Department: {trainee.Department?.Name}";
            byte[] qrImage = QRCodeHelper.GenerateQRCode(qrText);
            string base64Image = Convert.ToBase64String(qrImage);

            ViewBag.QRImage = $"data:image/png;base64,{base64Image}";
            ViewBag.Name = trainee.Name;

            return View("QR");
        }
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> QR()
        {
            var userId = _userManager.GetUserId(User);
            var trainee = await _context.Trainees
                .Include(t => t.Department)
                .FirstOrDefaultAsync(t => t.UserId == userId);

            if (trainee == null)
            {
                ViewBag.ErrorMessage = "Sorry, trainee information is missing. Please complete your profile or contact the admin.";
                return View("QRMissing");
            }


            string qrText = $"Name: {trainee.Name}, ID: {trainee.Id}, Department: {trainee.Department?.Name}";
            byte[] qrImage = QRCodeHelper.GenerateQRCode(qrText);
            string base64Image = Convert.ToBase64String(qrImage);

            ViewBag.QRImage = $"data:image/png;base64,{base64Image}";
            ViewBag.Name = trainee.Name;

            return View("QR");
        }
        [Authorize(Roles = "Trainee,Admin")]
        public async Task<IActionResult> GeneratePdf(int id)
        {
            var result = await _context.crsResults
                .Include(r => r.trainee)
                .Include(r => r.course)
                .FirstOrDefaultAsync(r => r.TraineeId == id);

            if (result == null) return Content("Yor degree not load, check in anther time");

            var qrData = $"{result.trainee.Name} - {result.course.Name} - {result.Degree} ";
            var qrCodeBytes = QRCodeHelper.GenerateQRCode(qrData);

            var pdfBytes = _pdfService.GenerateCertificate(
                result.trainee.Name,
                result.course.Name,
                result.Degree,
                qrCodeBytes
            );

            return File(pdfBytes, "application/pdf", "certificate.pdf");
        }
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> AddCourse()
        {
            var courses = await _context.Courses.ToListAsync();
            var vm = new AddCourseVM { Courses = courses };
            return View("AddCourse", vm);
        }
        [HttpPost]
        [Authorize(Roles = "Trainee")]
        public async Task<IActionResult> AddCourse(AddCourseVM vm)
        {
            if (!ModelState.IsValid) return View("AddCourse", vm);

            var user = await _userManager.GetUserAsync(User);
            var trainee = await _context.Trainees.FirstOrDefaultAsync(t => t.UserId == user.Id);
            if (trainee == null)
            {
                TempData["Error"] = "User not found. Please register again.";
                return RedirectToAction("Index", "Home"); // أو أي View مناسبة
            }

            // تحقق من عدم التكرار
            var exists = await _context.crsResults.AnyAsync(r => r.TraineeId == trainee.Id && r.CourseId == vm.CourseId);
            if (exists)
            {
                ModelState.AddModelError("", "You already registered this course.");
                vm.Courses = await _context.Courses.ToListAsync();
                return View("AddCourse", vm);
            }

            // إضافة الكورس
            var result = new crsResult
            {
                TraineeId = trainee.Id,
                CourseId = vm.CourseId,
                Degree = 0 // أو خليها null لو لسه مفيش درجة
            };

            _context.crsResults.Add(result);
            await _context.SaveChangesAsync();

            return RedirectToAction("MyCourses", "CrsResult"); // Redirect to MyCourses action in CrsResultController
        }





    }
}