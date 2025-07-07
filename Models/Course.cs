using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Degree { get; set; }
        public string? CourseUrl { get; set; }
        public double MinDegree { get; set; }
        [ForeignKey("Department")]
        public int Deptid { get; set; }
        public ICollection<crsResult> results { get; set; }
        public ICollection<Instructor> Instructors { get; set; }

        public Department Department { get; set; }
        
    }
}