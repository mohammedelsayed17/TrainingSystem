using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.ViewModels
{
    public class DepartmentVM
    {
         public int Id { get; set; }

    [Required]
    [Display(Name = "Department Name")]
    public string Name { get; set; }

    [Display(Name = "Manager Name")]
    public string? Manager { get; set; }
    }
}