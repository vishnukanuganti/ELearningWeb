﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ELearningWeb.Models;
using ELearningWeb.Models.ViewModel;
using System.Reflection.Emit;

namespace ELearningWeb.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ClassCourse> ClassCourses { get; set; }
        public DbSet<ClassStudent> ClassStudents { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }

        public DbSet<Question> Questions { get; set; }
        public DbSet<QuizAttempt> QuizAttempts { get; set; }
        public DbSet<StudentCourseProgress> CourseProgress { get; set; }
        public DbSet<Review> Reviews { get; set; }


        public DbSet<DiscussionPost> DiscussionPosts { get; set; } // Add DiscussionPosts
        public DbSet<DiscussionReply> DiscussionReplies { get; set; }





        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // builder.Entity<ClassCourse>().HasKey(cc => new { cc.ClassId, cc.CourseId });
            //builder.Entity<ClassStudent>().HasKey(cs => new { cs.ClassId, cs.StudentId });
            //builder.Entity<StudentCourseProgress>().HasKey(scp => new { scp.StudentId, scp.CourseId });

            builder.Entity<ClassCourse>().HasKey(cc => new { cc.ClassId, cc.CourseId });
            builder.Entity<ClassCourse>()
                .HasOne(cc => cc.Class)
                .WithMany(c => c.ClassCourses)
                .HasForeignKey(cc => cc.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ClassCourse>()
                .HasOne(cc => cc.Course)
                .WithMany(c => c.ClassCourses)
                .HasForeignKey(cc => cc.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ClassStudent>().HasKey(cs => new { cs.ClassId, cs.StudentId });
            builder.Entity<ClassStudent>()
                .HasOne(cs => cs.Class)
                .WithMany(c => c.ClassStudents)
                .HasForeignKey(cs => cs.ClassId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<ClassStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.ClassStudents)
                .HasForeignKey(cs => cs.StudentId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.Entity<Quiz>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Title).IsRequired().HasMaxLength(100);
                entity.Property(q => q.ClassId).IsRequired();

                entity.HasOne(q => q.Class)
                    .WithMany(c => c.Quizzes)
                    .HasForeignKey(q => q.ClassId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(q => q.Questions)
                    .WithOne(q => q.Quiz)
                    .HasForeignKey(q => q.QuizId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(q => q.QuizAttempts)
                    .WithOne(a => a.Quiz)
                    .HasForeignKey(a => a.QuizId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Question>(entity =>
            {
                entity.HasKey(q => q.Id);
                entity.Property(q => q.Text).IsRequired().HasMaxLength(500);
                entity.Property(q => q.QuizId).IsRequired();
                entity.Property(q => q.Type).IsRequired().HasMaxLength(20);
                entity.Property(q => q.Options).HasMaxLength(500);
                entity.Property(q => q.CorrectAnswer).IsRequired().HasMaxLength(100);

                entity.HasOne(q => q.Quiz)
                    .WithMany(q => q.Questions)
                    .HasForeignKey(q => q.QuizId)
                    .OnDelete(DeleteBehavior.Cascade);
            });




            builder.Entity<QuizAttempt>(entity =>
            {
                entity.HasKey(qa => qa.Id);
                entity.Property(qa => qa.QuizId).IsRequired();
                entity.Property(qa => qa.StudentId).IsRequired();
                entity.Property(qa => qa.Answers).IsRequired();
                entity.Property(qa => qa.Grade).HasColumnType("int"); // Using int instead of decimal

                entity.HasOne(qa => qa.Quiz)
                    .WithMany(q => q.QuizAttempts)
                    .HasForeignKey(qa => qa.QuizId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(qa => qa.Student)
                    .WithMany(u => u.QuizAttempts)
                    .HasForeignKey(qa => qa.StudentId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<StudentCourseProgress>().HasKey(scp => new { scp.StudentId, scp.CourseId });
            builder.Entity<StudentCourseProgress>()
                .HasOne(scp => scp.Student)
                .WithMany(s => s.CourseProgress)
                .HasForeignKey(scp => scp.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<StudentCourseProgress>()
                .HasOne(scp => scp.Course)
                .WithMany(c => c.CourseProgress)
                .HasForeignKey(scp => scp.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Review>()
                .HasOne(r => r.Course).WithMany(c => c.Reviews).HasForeignKey(r => r.CourseId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Review>()
                .HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DiscussionPost>()
                .HasOne(dp => dp.Class) // Changed from Course to Class
                .WithMany(c => c.DiscussionPosts) // Assumes Class has DiscussionPosts collection
                .HasForeignKey(dp => dp.ClassId) // Matches the foreign key
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DiscussionPost>()
                .HasOne(dp => dp.User)
                .WithMany()
                .HasForeignKey(dp => dp.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure DiscussionReply
            builder.Entity<DiscussionReply>()
                .HasOne(dr => dr.Post)
                .WithMany(dp => dp.Replies)
                .HasForeignKey(dr => dr.PostId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<DiscussionReply>()
                .HasOne(dr => dr.User)
                .WithMany()
                .HasForeignKey(dr => dr.UserId)
                .OnDelete(DeleteBehavior.Restrict);






            // Seed pre-defined courses
            builder.Entity<Course>().HasData(
                new Course { Id = 1, Name = "Introduction to C#", Subject = "Programming", Overview = "Learn C# basics", Syllabus = "Variables, Loops, OOP", Prerequisites = "None"},
                new Course { Id = 2, Name = "Web Development with ASP.NET", Subject = "Web Development", Overview = "Build web apps", Syllabus = "MVC, Razor, EF Core", Prerequisites = "C# basics" },
                new Course { Id = 3, Name = "Data Structures", Subject = "Computer Science", Overview = "Core CS concepts", Syllabus = "Arrays, Linked Lists, Trees", Prerequisites = "Programming basics" },
                new Course { Id = 4, Name = "SQL Fundamentals", Subject = "Database", Overview = "Database basics", Syllabus = "Queries, Joins, Indexes", Prerequisites = "None" }
            );
        }
    }
}
