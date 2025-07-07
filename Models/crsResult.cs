using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.Models
{
    public class crsResult
    {
        public int Id { get; set; }
        public double Degree { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        [ForeignKey("Trainee")]
        public int TraineeId { get; set; }
        public Course? course { get; set; }
        public Trainee? trainee { get; set; }
    }
}