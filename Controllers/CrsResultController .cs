using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrainingSystem.Services;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    // [Route("[controller]")]
    public class CrsResultController : Controller
    {
        private readonly ResultService service;

        public CrsResultController(ResultService service)
        {
            this.service = service;
        }
        public async Task<IActionResult> Index()
        {
            var results = await service.GetAllAsync();
            return View("Index",results);
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
                return View("Create",vm);
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


    }
}