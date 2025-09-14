using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Views.Trainee
{
    public class AddCourse : PageModel
    {
        private readonly ILogger<AddCourse> _logger;

        public AddCourse(ILogger<AddCourse> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}