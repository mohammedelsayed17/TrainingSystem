using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Views.CrsResult
{
    public class MyCourses : PageModel
    {
        private readonly ILogger<MyCourses> _logger;

        public MyCourses(ILogger<MyCourses> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}