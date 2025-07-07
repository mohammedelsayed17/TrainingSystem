using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;
using TrainingSystem.Repositories.Interfaces;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Repositories
{
    public interface IDepartmentRepository: IGenericRepository<Department>
    {
        
    }
}