using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Views.Shared
{
    public class Mylayout : PageModel
    {
        private readonly ILogger<Mylayout> _logger;

        public Mylayout(ILogger<Mylayout> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}