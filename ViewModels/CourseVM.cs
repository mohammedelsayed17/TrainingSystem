using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;

namespace TrainingSystem.ViewModels
{
    public class CourseVM
    { public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    [Display(Name = "Max Degree")]
    public double Degree { get; set; }
    public string? CourseUrl { get; set; }

    [Required]
    [Display(Name = "Min Degree")]
    public double MinDegree { get; set; }

    [Required]
    [Display(Name = "Department")]
    public int Deptid { get; set; }

    public IEnumerable<Department>? Departments { get; set; }


    }
}