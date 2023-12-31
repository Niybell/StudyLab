using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyLab.Models.EntityFramework.Contexts;
using StudyLab.Models.EntityFramework.Repositories;
using StudyLab.Models.ServerModels.Courses;
using StudyLab.Models.ServerModels.User;
using StudyLab.Services.Implementations;
using StudyLab.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();   
builder.Services.AddDbContext<AuthDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AppConnectionString")));
builder.Services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<AuthDBContext>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IBaseRepository<Course>, CourseRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();
