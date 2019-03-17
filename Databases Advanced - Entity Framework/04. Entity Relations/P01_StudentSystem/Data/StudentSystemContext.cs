using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem.Data
{
    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {
            
        }

        public StudentSystemContext(DbContextOptions options)
            :base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(k => k.StudentId);

                entity.Property(x => x.Name)
                    .HasMaxLength(100)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(x => x.PhoneNumber)
                    .HasMaxLength(10)
                    .IsFixedLength()
                    .IsRequired(false);

                entity.Property(x => x.RegisteredOn)
                    .IsRequired();

                entity.Property(x => x.Birthday)
                    .IsRequired(false);

                entity.HasMany(x => x.HomeworkSubmissions)
                    .WithOne(x => x.Student)
                    .HasForeignKey(x => x.StudentId);

            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(x => x.CourseId);

                entity.Property(x => x.Name)
                    .HasMaxLength(80)
                    .IsUnicode()
                    .IsRequired();

                entity.Property(x => x.Description)
                    .IsUnicode()
                    .IsRequired(false);

                entity.Property(x => x.StartDate)
                    .IsRequired();

                entity.Property(x => x.EndDate)
                    .IsRequired();

                entity.Property(x => x.Price)
                    .IsRequired();

                entity.HasMany(x => x.HomeworkSubmissions)
                    .WithOne(x => x.Course)
                    .HasForeignKey(x => x.CourseId);


                entity.HasMany(x => x.Resources)
                    .WithOne(x => x.Course)
                    .HasForeignKey(x => x.CourseId);
            });

            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(x => x.ResourceId);

                entity.Property(x => x.Name)
                    .HasMaxLength(50)
                    .IsUnicode();

                entity.Property(x => x.Url)
                    .IsUnicode(false);

            });

            modelBuilder.Entity<Homework>(entity =>
            {
                entity.HasKey(x => x.HomeworkId);

                entity.Property(x => x.Content)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StudentCourse>(entity =>
            {
                entity.HasKey(x => new {x.StudentId, x.CourseId});

                entity.HasOne(x => x.Student)
                    .WithMany(x=> x.CourseEnrollments)
                    .HasForeignKey(x => x.StudentId);

                entity.HasOne(x => x.Course)
                    .WithMany(x => x.StudentsEnrolled)
                    .HasForeignKey(x => x.CourseId);
            });
        }
      
    }
}
