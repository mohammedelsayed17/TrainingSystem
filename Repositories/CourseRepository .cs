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

    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext context;

        public CourseRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(Course entity)
        {
            await context.Courses.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var course = await GetByIdAsync(id);
        if (course != null)
        {
            context.Courses.Remove(course);
        }
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await context.Courses.Include(c => c.Department).ToListAsync();
        }

        public async Task<Course> GetByIdAsync(int id)
        {
            return await context.Courses.Include(c => c.Department)
                                     .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChangesAsync()
        {
              await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Course entity)
        {
              context.Courses.Update(entity);
        }
    }
}