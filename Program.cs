using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TrainingSystem.Data;
using TrainingSystem.Models;
using TrainingSystem.Repositories;
using TrainingSystem.Repositories.Interfaces;
using TrainingSystem.Services;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
QuestPDF.Settings.License = LicenseType.Community;
// add conecction string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // لو المستخدم مش مسجل الدخول
    options.AccessDeniedPath = "/Account/AccessDenied"; // لو مش معاه صلاحية
});
// add identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();
// Add services to the container.
builder.Services.AddControllersWithViews();
//Add custom service
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<IResultRepository, ResultRepository>();
// builder.Services.AddScoped<InstructorService>();
builder.Services.AddScoped<InstructorRepository>();
builder.Services.AddScoped<InstructorService>();
builder.Services.AddScoped<DepartmentService>();
builder.Services.AddScoped<CourseService>();
builder.Services.AddScoped<TraineeService>();
builder.Services.AddScoped<ResultService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<PdfCourseService>();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await AppDbContext.SeedRolesAsync(services);
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

