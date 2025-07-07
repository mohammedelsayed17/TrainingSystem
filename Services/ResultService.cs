using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.Repositories.Interfaces;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Services
{
    public class ResultService
    {
        private readonly IResultRepository repo;
        private readonly AppDbContext context;

        public ResultService(IResultRepository repo, AppDbContext context)
        {
            this.repo = repo;
            this.context = context;
        }
        public async Task<IEnumerable<crsResult>> GetAllAsync()
        {
            return await repo.GetAllAsync();
        }
        public async Task<crsResult?> GetByIdAsync(int id)
        {
            return await repo.GetByIdAsync(id);
        }
        public async Task AddAsync(CrsResultVM vm)
        {
            var result = new crsResult
            {
                Degree = vm.Degree,
                CourseId = vm.CourseId,
                TraineeId = vm.TraineeId
            };

            await repo.AddAsync(result);
            await repo.SaveChangesAsync();
        }
        public async Task UpdateAsync(CrsResultVM vm)
        {
            var result = await repo.GetByIdAsync(vm.Id);
            if (result != null)
            {
                result.Degree = vm.Degree;
                result.CourseId = vm.CourseId;
                result.TraineeId = vm.TraineeId;

                await repo.UpdateAsync(result);
                await repo.SaveChangesAsync();
            }
        }
        public async Task DeleteAsync(int id)
        {
            await repo.DeleteAsync(id);
            await repo.SaveChangesAsync();
        }
        public async Task<CrsResultVM> PrepareVMAsync()
        {
            return new CrsResultVM
            {
                Courses = await context.Courses.ToListAsync(),
                Trainees = await context.Trainees.ToListAsync()
            };
        }
    }
}