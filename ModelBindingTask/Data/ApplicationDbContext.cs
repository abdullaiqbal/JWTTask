using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModelBindingTask.Models;

namespace ModelBindingTask.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
     
        public DbSet<Student> Student { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<StudentCourse> StudentCourse { get; set; }

        //public DbSet<User> Users { get; set; }

      
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>().HasKey(SC => new { SC.CourseId, SC.StudentId });



            modelBuilder.Entity<StudentCourse>()
                .HasOne<Student>(sc => sc.Student)
                .WithMany(s => s.StudentCourse)
                .HasForeignKey(sc => sc.StudentId);


            modelBuilder.Entity<StudentCourse>()
                .HasOne<Course>(sc => sc.Course)
                .WithMany(s => s.StudentCourse)
                .HasForeignKey(sc => sc.CourseId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
