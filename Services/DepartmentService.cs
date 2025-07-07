using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;
using TrainingSystem.Repositories;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Services
{
    public class DepartmentService
    {

        private readonly IDepartmentRepository repo;
        public DepartmentService(IDepartmentRepository repository)
        {
            repo = repository;

        }
         public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await repo.GetAllAsync();
        }
        public async Task<DepartmentVM> GetVMByIdAsync(int id)
        {
            var dept = await repo.GetByIdAsync(id);
            if (dept == null) return null;
            return new DepartmentVM
            {
                Id = dept.Id,
                Name = dept.Name,
                Manager = dept.Manager
            };
        }
        public async Task AddAsync(DepartmentVM vm)
        {
            var dept = new Department
            {
                Name = vm.Name,
                Manager = vm.Manager

            };
            await repo.AddAsync(dept);
            await repo.SaveChangesAsync();
        }
        public async Task UpdateAsync(DepartmentVM vm)
        {
            var dept = new Department
            {
                Id = vm.Id,
                Name = vm.Name,
                Manager = vm.Manager


            };
            await repo.UpdateAsync(dept);
            await repo.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            await repo.DeleteAsync(id);
            await repo.SaveChangesAsync();
        }
    }
}