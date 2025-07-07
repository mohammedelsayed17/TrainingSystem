using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Views.Role
{
    public class Assign : PageModel
    {
        private readonly ILogger<Assign> _logger;

        public Assign(ILogger<Assign> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}