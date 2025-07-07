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
    public class TraineeService
    {
        private readonly ITraineeRepository repo;
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment env;

        public object Trainees { get; internal set; }
        public object CrsResults { get; internal set; }

        public TraineeService(ITraineeRepository repo, AppDbContext context, IWebHostEnvironment env)
        {
            this.repo = repo;
            this.context = context;
            this.env = env;
        }
        public async Task<IEnumerable<Trainee>> GetAllAsync() => await repo.GetAllAsync();
        public async Task<Trainee?> GetByIdAsync(int id) => await repo.GetByIdAsync(id);
        public async Task<TraineeVM> PrepareVMAsync()
        {
            return new TraineeVM
            {
                Departments = await context.Departments.ToListAsync()
            };
        }
        public async Task AddAsync(TraineeVM vm)
        {
            var fileName = "";
            if (vm.ImageFile != null)
            {
                fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var path = Path.Combine(env.WebRootPath, "images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);
            }

            var trainee = new Trainee
            {
                Name = vm.Name,
                Address = vm.Address,
                DeptID = vm.DeptID,
                Grade = vm.Grade,
                ImageUrl = fileName
            };

            await repo.AddAsync(trainee);
            await repo.SaveChangesAsync();
        }
        public async Task UpdateAsync(TraineeVM vm)
        {
            var trainee = await repo.GetByIdAsync(vm.Id);
            if (trainee == null) return;

            if (vm.ImageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var path = Path.Combine(env.WebRootPath, "images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);
                trainee.ImageUrl = fileName;
            }

            trainee.Name = vm.Name;
            trainee.Address = vm.Address;
            trainee.Grade = vm.Grade;
            trainee.DeptID = vm.DeptID;

            await repo.UpdateAsync(trainee);
            await repo.SaveChangesAsync();
        }
     public async Task DeleteAsync(int id)
    {
        await repo.DeleteAsync(id);
        await repo.SaveChangesAsync();
    }



    }
}