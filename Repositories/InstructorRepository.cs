using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.Repositories.Interfaces;

namespace TrainingSystem.Repositories
{
    public class InstructorRepository : IInstructorRepository
    {
        AppDbContext context;
        public InstructorRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Instructor entity)
        {
             await context.Instructors.AddAsync(entity);
            
        }

        public async Task DeleteAsync(int id)
        {
            var instructor = await GetByIdAsync(id);
            if (instructor != null)
            {
                context.Instructors.Remove(instructor);
            }
            // var instructor = await context.Instructors.FirstOrDefaultAsync()
            // if (instructor != null)
            // {
            //     context.Instructors.Remove(instructor);
            // }
            // else
            // {
            //     throw new KeyNotFoundException("Instructor not found");
            // }
        }

        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await context.Instructors.Include(i => i.Department).Include(i=>i.Course).ToListAsync();
        }

        public async Task<Instructor?> GetByIdAsync(int id)
        {
           return await context.Instructors.Include(i => i.Department).FirstOrDefaultAsync(i => i.Id == id);
        }

        public  async Task SaveChangesAsync()
        {
              await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Instructor entity)
        {
            var existingInstructor = await GetByIdAsync(entity.Id);
            if (existingInstructor != null)
            {
                 context.Instructors.Update(entity);
            }
            // throw new NotImplementedException();
        }
    }
}