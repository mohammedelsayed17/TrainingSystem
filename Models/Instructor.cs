using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.Models
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
        public string Image { get; set; }
        public string? LinkedInUrl { get; set; }
        public string Address { get; set; }
        [ForeignKey("Department")]
        public int Deptid { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Department Department { get; set; }
        public Course? Course { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser? User { get; set; }

    }
}