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
    public class CourseService
    {
        private readonly ICourseRepository repo;
        private readonly AppDbContext context;
        public CourseService(ICourseRepository repo, AppDbContext context)
        {
            this.repo = repo;
            this.context = context;
        }

        public async Task<IEnumerable<Course>> GetAllAsync()
        {
            return await repo.GetAllAsync();
        }
        public async Task<CourseVM> PrepareVMAsync()
        {
            return new CourseVM
            {
                Departments = await context.Departments.ToListAsync()
            };
        }
        public async Task AddAsync(CourseVM vm)
        {
            var course = new Course
            {
                Name = vm.Name,
                Degree = vm.Degree,
                MinDegree = vm.MinDegree,
                Deptid = vm.Deptid,
                CourseUrl = vm.CourseUrl
            };

            await repo.AddAsync(course);
            await repo.SaveChangesAsync();
        }
        public async Task<CourseVM?> GetByIdAsync(int id)
        {
            var course = await repo.GetByIdAsync(id);
            if (course == null) return null;

            return new CourseVM
            {
                Id = course.Id,
                Name = course.Name,
                Degree = course.Degree,
                MinDegree = course.MinDegree,
                Deptid = course.Deptid,
                CourseUrl = course.CourseUrl,
                Departments = await context.Departments.ToListAsync()
            };
        }
        public async Task UpdateAsync(CourseVM vm)
        {
            var course = await repo.GetByIdAsync(vm.Id);
            if (course == null) return;

            course.Name = vm.Name;
            course.Degree = vm.Degree;
            course.MinDegree = vm.MinDegree;
            course.Deptid = vm.Deptid;
            course.CourseUrl = vm.CourseUrl;

            await repo.UpdateAsync(course);
            await repo.SaveChangesAsync();
        }
     public async Task DeleteAsync(int id)
    {
        await repo.DeleteAsync(id);
        await repo.SaveChangesAsync();
    }






    }
}