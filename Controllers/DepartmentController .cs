using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainingSystem.Data;
using TrainingSystem.Services;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Controllers
{
    // [Route("[controller]")]
    public class DepartmentController : Controller
    {
        private DepartmentService Service;
        private readonly AppDbContext _context;
        public DepartmentController(DepartmentService service, AppDbContext context)
        {
            this.Service = service;
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            var departments = await Service.GetAllAsync();
            return View("Index", departments);
        }
        public IActionResult Create()
        {
            return View("Create");
        }
        [HttpPost]
        public async Task<IActionResult> Create(DepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View("Create", vm);

            await Service.AddAsync(vm);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Edit(int id)
        {
            var vm = await Service.GetVMByIdAsync(id);
            if (vm == null) return NotFound();

            return View("Edit", vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(DepartmentVM vm)
        {
            if (!ModelState.IsValid)
                return View("Edit", vm);
            var duplicate = await _context.Departments
        .FirstOrDefaultAsync(d => d.Name == vm.Name && d.Id != vm.Id);
            if (duplicate != null)
            {
                ModelState.AddModelError(string.Empty, "This department already exists.");
                return View("Edit", vm);
            }

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