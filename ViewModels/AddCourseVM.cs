using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;

namespace TrainingSystem.ViewModels
{
    public class AddCourseVM
    {
         [Required]
    public int CourseId { get; set; }

    public List<Course>? Courses { get; set; }
    }
}