using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;

namespace TrainingSystem.ViewModels
{
    public class CrsResultVM
    {
         public int Id { get; set; }

        [Required]
        [Display(Name = "Trainee")]
        public int TraineeId { get; set; }

        [Required]
        [Display(Name = "Course")]
        public int CourseId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Enter a valid degree")]
        public double Degree { get; set; }

        public IEnumerable<Trainee>? Trainees { get; set; }
        public IEnumerable<Course>? Courses { get; set; }
    }
}