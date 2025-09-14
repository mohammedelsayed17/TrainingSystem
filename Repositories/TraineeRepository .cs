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
    public class TraineeRepository : ITraineeRepository
    {
        private readonly AppDbContext context;

        public TraineeRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(Trainee entity)
        {
            await context.Trainees.AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var trainee = await context.Trainees
       .Include(t => t.CrsResults)
       .FirstOrDefaultAsync(t => t.Id == id);
            //  var trainee = await GetByIdAsync(id);
            if (trainee == null) return;
            // context.Trainees.Remove(trainee);
            context.crsResults.RemoveRange(trainee.CrsResults);
            context.Trainees.Remove(trainee);
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == trainee.UserId);
            if (user != null)
                context.Users.Remove(user);


        }

        public async Task<IEnumerable<Trainee>> GetAllAsync()
        {
            return await context.Trainees.Include(t => t.Department).ToListAsync();
        }

        public async Task<Trainee> GetByIdAsync(int id)
        {
            return await context.Trainees.Include(t => t.Department).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Trainee entity)
        {
            context.Trainees.Update(entity);
        }
    }
}