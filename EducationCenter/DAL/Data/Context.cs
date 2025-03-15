using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }
        public DbSet<CourseSchedule> CourseSchedules { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<Message> Messages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.IsActive)
                .HasDefaultValue(true);

            // Define relationships
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.User)
                .WithMany(u => u.Enrollments)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade); // Keep this as Cascade

            modelBuilder.Entity<Discussion>()
                .HasOne(d => d.Course)
                .WithMany(c => c.Discussions)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Discussion>()
                .HasOne(d => d.User)
                .WithMany(u => u.Discussions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict to avoid cycles or multiple cascade paths

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Discussion)
                .WithMany(d => d.Messages)
                .HasForeignKey(m => m.DiscussionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Change to Restrict to avoid multiple cascade paths
        }
    }
}
