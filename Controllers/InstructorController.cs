using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainingSystem.Helpers;
using TrainingSystem.Services;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InstructorController : Controller
    {
        private readonly InstructorService _service;

        public InstructorController(InstructorService service)
        {
            _service = service;
        }

        public async Task<IActionResult> Index()
        {
            var instructors = await _service.GetAllAsync();
            return View("Index", instructors);
        }

        public async Task<IActionResult> Create()
        {
            var vm = await _service.PrepareVMAsync();
            return View("Create", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InstructorVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = await _service.GetDepartmentsAsync();
                vm.courses = await _service.GetCoursesAsync();
                return View("Create", vm);
            }

            await _service.AddAsync(vm);
            return RedirectToAction("Index");
        }
    [Authorize(Roles = "Instructor,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await _service.GetVMByIdAsync(id);
            if (vm == null) return NotFound();
            return View("Edit", vm);
        }
[Authorize(Roles = "Instructor,Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(InstructorVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Departments = await _service.GetDepartmentsAsync();
                vm.courses = await _service.GetCoursesAsync();
                return View("Edit", vm);
            }

            await _service.UpdateAsync(vm);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Details(int id)
        {
            var instructor = await _service.GetVMByIdAsync(id);
            if (instructor == null) return NotFound();

            string url = instructor.LinkedInUrl ?? Url.Action("Details", "Instructor", new { id }, Request.Scheme);
            ViewBag.QRCode = QRCodeHelper.GenerateQRCode(url);


            return View("Details", instructor);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var instructor = await _service.GetVMByIdAsync(id);
            if (instructor == null)
                return NotFound();

            return View("Delete", instructor);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


    }
}
