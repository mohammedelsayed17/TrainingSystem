using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace TrainingSystem.Views.Trainee
{
    public class QRMissing : PageModel
    {
        private readonly ILogger<QRMissing> _logger;

        public QRMissing(ILogger<QRMissing> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}