using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;

namespace TrainingSystem.ViewModels
{
    public class InstructorVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Instructor Name")]
        public string Name { get; set; }
        [Required]
        [Range(1000, double.MaxValue, ErrorMessage = "Salary must be > 1000")]
        public double Salary { get; set; }
        [Required]
        [Display(Name = "LinkedIn URL or any other social media")]
        public string? LinkedInUrl { get; set; }
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Department")]
        public int Deptid { get; set; }
        public IEnumerable<Department>? Departments { get; set; }
        public int CourseId { get; set; }
          public IEnumerable<Course>? courses { get; set; }
        // public List<Course>? Courses { get;  set; }
        public IFormFile? ImageFile { get; set; }
        public string? Image { get; set; }
    }
}