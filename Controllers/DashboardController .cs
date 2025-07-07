using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TrainingSystem.Data;

using TrainingSystem.ViewModels;
namespace TrainingSystem.Controllers
{
    //[Route("[controller]")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var vm = new DashboardVM
            {
                InstructorCount = await _context.Instructors.CountAsync(),
                TraineeCount = await _context.Trainees.CountAsync(),
                CourseCount = await _context.Courses.CountAsync(),
                AverageDegree = await _context.crsResults.AverageAsync(r => (double?)r.Degree),
                MaxDegree = await _context.crsResults.MaxAsync(r => (double?)r.Degree),
                MinDegree = await _context.crsResults.MinAsync(r => (double?)r.Degree)
            };

            return View("Index", vm);
        }
    }
}