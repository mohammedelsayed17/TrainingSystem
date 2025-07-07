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
                return NotFound();

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



    }
}