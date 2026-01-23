using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SistepedApi.Models;

namespace SistepedApi.Infra.Data
{
    public class SistepedDbContext : DbContext
    {
        public SistepedDbContext(DbContextOptions<SistepedDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserCredential> UserCredentials { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Grid> Grids { get; set; }
        public DbSet<GridGrade> GridGrades { get; set; }
        public DbSet<GridClass> GridClasses { get; set; }
        public DbSet<ClassTeacher> ClassTeachers { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentGrade> StudentGrades { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<StudentActivity> StudentActivities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly();
        }
    }
}
