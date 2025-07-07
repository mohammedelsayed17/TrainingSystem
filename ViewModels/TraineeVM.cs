using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;

namespace TrainingSystem.ViewModels
{
    public class TraineeVM
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Trainee Name")]
        public string Name { get; set; }
        public string? Address { get; set; }
        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }
        public string? ImageUrl { get; set; }
        public string? Grade { get; set; }
        [Display(Name = "Department")]
        [Required]
        public int DeptID { get; set; }
     public IEnumerable<Department>? Departments { get; set; }



    }
}