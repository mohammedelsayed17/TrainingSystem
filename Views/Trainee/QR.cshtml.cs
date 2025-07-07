using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Views.Trainee
{
    public class QR : PageModel
    {
        private readonly ILogger<QR> _logger;

        public QR(ILogger<QR> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}