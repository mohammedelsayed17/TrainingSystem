using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TrainingSystem.Models;
using Microsoft.AspNetCore.Http;
namespace TrainingSystem.ViewModels
{
    public class RegisterVM
    {
          [Required]
    public string UserName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, DataType(DataType.Password)]
    public string Password { get; set; }

    [DataType(DataType.Password), Compare("Password")]
    public string ConfirmPassword { get; set; }

    [Required]
    public string Role { get; set; }

    // === Shared ===
    public string? Address { get; set; }

    // === Trainee-specific ===
    public int DeptID { get; set; }
    public string? Grade { get; set; }

    // === Instructor-specific ===
    public DateTime? HireDate { get; set; }  // مثال لو عندك
 public IFormFile? ImageFile { get; set; } 
    // Department dropdown
    public List<Department>? Departments { get; set; }

    }
}