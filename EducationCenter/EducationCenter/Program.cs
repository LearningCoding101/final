using BLL.Interface;
using BLL.Service;
using DAL.Data;
using DAL.Interface;
using DAL.UnitOfWork;
using EducationCenter.BLL.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EducationCenter
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Register the DbContext
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Register the UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register the JwtService
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddScoped<IPasswordHasher<DAL.Entities.User>, PasswordHasher<DAL.Entities.User>>();

            // Register the UserService
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
            builder.Services.AddScoped<ICourseMaterialService, CourseMaterialService>();
            builder.Services.AddScoped<IDiscussionService, DiscussionService>();
            builder.Services.AddScoped<IMessageService, MessageService>();
            builder.Services.AddScoped<INewsService, NewsService>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/User/Login";
                    options.AccessDeniedPath = "/User/AccessDenied";
                });


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}