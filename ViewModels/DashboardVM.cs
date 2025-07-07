using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrainingSystem.ViewModels
{
    public class DashboardVM
    {
        public int InstructorCount { get; set; }
        public int TraineeCount { get; set; }
        public int CourseCount { get; set; }
        public double? AverageDegree { get; set; }
        public double? MaxDegree { get; set; }
        public double? MinDegree { get; set; }
    
    }
}