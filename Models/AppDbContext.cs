using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace StudentAttendanceManagementSystem.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Branch> Branch { get; set; }
        public DbSet<Subject> Subject { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<Login> Login { get; set; }
        public DbSet<Admin> Admin { get; set; }
        public DbSet<Faculty> Faculty { get; set; }
        public DbSet<StudentSubject> StudentSubject { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Subject>()
            .HasOne(s => s.Faculty)
            .WithOne(f => f.Subject)
            .HasForeignKey<Faculty>(f => f.SubjectId)
            .OnDelete(DeleteBehavior.Restrict);
            //builder.Entity<Login>()
            //    .HasIndex(u => u.Username)
            //    .IsUnique();
            //builder.Entity<Student>()
            //    .HasIndex(u => u.UniversityId)
            //    .IsUnique();

            //builder.Entity<Subject>()
            //    .HasIndex(u => u.SubjectCode)
            //    .IsUnique();
            //builder.Entity<StudentSubject>()
            //    .HasKey(ss => new { ss.StudentId, ss.SubjectId });

            builder.Entity<StudentSubject>()
                .HasOne(ss => ss.Student)
                .WithMany(st => st.StudentSubjects)
                .HasForeignKey(ss => ss.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<StudentSubject>()
                .HasOne(ss => ss.Subject)
                .WithMany(sub => sub.StudentSubjects)
                .HasForeignKey(ss => ss.SubjectId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Admin>().HasKey(x => x.Id);
            builder.Entity<Branch>().HasKey(x => x.Id);
            builder.Entity<Faculty>().HasKey(x => x.Id);
            builder.Entity<Login>().HasKey(x => x.Id);
            builder.Entity<Student>().HasKey(x => x.Id);
            builder.Entity<Subject>().HasKey(x => x.Id);
            base.OnModelCreating(builder);
        }
    }
}

