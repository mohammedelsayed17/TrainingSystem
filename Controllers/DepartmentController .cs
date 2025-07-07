using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TrainingSystem.Services;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    // [Route("[controller]")]
    public class DepartmentController : Controller
    {
        private DepartmentService Service;
        public DepartmentController(DepartmentService service)
        {
            this.Service = service;

        }
        public async Task<IActionResult> Index()
        {
            var departments = await Service.GetAllAsync();
            return View("Index",departments);
        }
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View("Create",vm);

            await Service.AddAsync(vm);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await Service.GetVMByIdAsync(id);
            if (vm == null) return NotFound();

            return View("Edit",vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View("Edit",vm);

            await Service.UpdateAsync(vm);
            return RedirectToAction(nameof(Index));
        }
         public async Task<IActionResult> Delete(int id)
        {
            await Service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }


       
    }
}