using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Manager { get; set; }

       public ICollection<Instructor> Instructors { get; set; }
        public ICollection<Trainee> Trainees { get; set; }
        public ICollection<Course> Courses { get; set; }
    }
}