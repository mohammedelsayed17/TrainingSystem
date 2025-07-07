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
    public class ResultRepository : IResultRepository
    {
         private readonly AppDbContext context;

        public ResultRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(crsResult entity)
        {
            await context.crsResults.AddAsync(entity);
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<crsResult>> GetAllAsync()
        {
              return await context.crsResults
                .Include(r => r.course)
                .Include(r => r.trainee)
                .ToListAsync();
        }

        public async Task<crsResult?> GetByIdAsync(int id)
        {
            return await context.crsResults
               .Include(r => r.course)
               .Include(r => r.trainee)
               .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task SaveChangesAsync()
        {
             await  context.SaveChangesAsync();
        }

        public async Task UpdateAsync(crsResult entity)
        {
             context.crsResults.Update(entity);
        }
    }
}