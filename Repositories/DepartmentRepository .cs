using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;

namespace TrainingSystem.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
         AppDbContext context;
        public DepartmentRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Department entity)
        {
            await context.Departments.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var department = await GetByIdAsync(id);
            if (department != null)
            {
                context.Departments.Remove(department);
            }
            // return Task.CompletedTask;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
             return await context.Departments.ToListAsync();
        }

        public async Task<Department> GetByIdAsync(int id)
        {
             return await context.Departments.FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task SaveChangesAsync()
        {
           await  context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department entity)
        {
            var existingDepartment = await GetByIdAsync(entity.Id);
            if (existingDepartment != null)
            {
             context.Departments.Update(entity);
            }
           
        }
    }
}