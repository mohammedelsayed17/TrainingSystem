using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.Repositories;
using TrainingSystem.ViewModels;

namespace TrainingSystem.Services
{
    public class InstructorService
    {
        private readonly InstructorRepository _repo;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public InstructorService(InstructorRepository repo, AppDbContext context, IWebHostEnvironment env)
        {
            _repo = repo;
            _context = context;
            _env = env;
        }

        // Get All Instructors
        public async Task<IEnumerable<Instructor>> GetAllAsync()
        {
            return await _repo.GetAllAsync();
        }

        // Add Instructor with image
        public async Task AddAsync(InstructorVM vm)
        {
            var instructor = new Instructor
            {
                Name = vm.Name,
                Salary = vm.Salary,
                Address = vm.Address,
                Deptid = vm.Deptid,
                CourseId = vm.CourseId,
                LinkedInUrl = vm.LinkedInUrl
            };

            if (vm.ImageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);
                instructor.Image = fileName;
            }

            await _repo.AddAsync(instructor);
            await _repo.SaveChangesAsync();
        }

        // Prepare ViewModel for Create
        public async Task<InstructorVM> PrepareVMAsync()
        {
            return new InstructorVM
            {
                Departments = await _context.Departments.ToListAsync(),
                courses = await _context.Courses.ToListAsync()
            };
        }

        // Get Departments only
        public async Task<IEnumerable<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }
        // Get Courses only
        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            return await _context.Courses.ToListAsync();
        }

        // Get ViewModel by Id for Edit
        public async Task<InstructorVM?> GetVMByIdAsync(int id)
        {
            var instructor = await _repo.GetByIdAsync(id);
            if (instructor == null) return null;

            return new InstructorVM
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Salary = instructor.Salary,
                Address = instructor.Address,
                Deptid = instructor.Deptid,
                CourseId = instructor.CourseId,
                Image = instructor.Image,
                LinkedInUrl = instructor.LinkedInUrl,
                Departments = await _context.Departments.ToListAsync(),
                courses = await _context.Courses.ToListAsync()
            };
        }

        // Update Instructor
        public async Task UpdateAsync(InstructorVM vm)
        {
            var instructor = await _repo.GetByIdAsync(vm.Id);
            if (instructor == null) return;

            instructor.Name = vm.Name;
            instructor.Salary = vm.Salary;
            instructor.Address = vm.Address;
            instructor.Deptid = vm.Deptid;
            instructor.CourseId = vm.CourseId;
            instructor.LinkedInUrl = vm.LinkedInUrl;

            if (vm.ImageFile != null)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(vm.ImageFile.FileName);
                var path = Path.Combine(_env.WebRootPath, "images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await vm.ImageFile.CopyToAsync(stream);
                instructor.Image = fileName;
            }

            await _repo.UpdateAsync(instructor);
            await _repo.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
{
    await _repo.DeleteAsync(id);
    await _repo.SaveChangesAsync();
}
    }
}
