using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Views.Course
{
    public class CourseCard : PageModel
    {
        private readonly ILogger<CourseCard> _logger;

        public CourseCard(ILogger<CourseCard> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}