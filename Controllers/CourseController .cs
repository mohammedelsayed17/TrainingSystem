using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrainingSystem.Helpers;
using TrainingSystem.Services;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
[Authorize(Roles = "Admin,Instructor")]
    public class CourseController : Controller
    {
        private readonly CourseService service;
        private readonly PdfCourseService  _pdfService;
        public CourseController(CourseService service, PdfCourseService  pdfService)
        {
            this.service = service;
            _pdfService = pdfService;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await service.GetAllAsync();
            return View("Index", courses);
        }
        public async Task<IActionResult> Create()
        {
            var vm = await service.PrepareVMAsync();
            return View("Create", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CourseVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = (await service.PrepareVMAsync()).Departments;
                return View("Create", vm);
            }

            await service.AddAsync(vm);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await service.GetByIdAsync(id);
            if (vm == null) return NotFound();
            return View("Edit", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CourseVM vm)
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
        public async Task<IActionResult> Details(int id)
        {
            var course = await service.GetByIdAsync(id);
            if (course == null) return NotFound();

            string qrLink = course.CourseUrl ?? Url.Action("Details", "Course", new { id }, Request.Scheme);
            ViewBag.QRCode = QRCodeHelper.GenerateQRCode(qrLink);

            return View("Details", course);
        }

        public async Task<IActionResult> GeneratePdf(int id)
        {
            var course = await service.GetByIdAsync(id); // أو mapping إلى CourseVM
            if (course == null) return NotFound();

            string link = course.CourseUrl ?? Url.Action("Details", "Course", new { id }, Request.Scheme);
            byte[] qr = QRCodeHelper.GenerateQRCode(link);

            string deptName = course.Departments?.FirstOrDefault(d => d.Id == course.Deptid)?.Name ?? "N/A";

            byte[] pdfBytes = _pdfService.GenerateCoursePdf(course.Name, course.Degree, deptName, qr);
            return File(pdfBytes, "application/pdf", "CourseInfo.pdf");
        }





    }
}