using System;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;

namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main(string[] args)
        {       
            using (StudentSystemContext context = new StudentSystemContext())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();
            }
        }

        public static void Seed(StudentSystemContext dbContext)
        {
            var students = new[]
            {
                new Student
                {
                    Birthday = new DateTime(1995, 10, 06),
                    Name = "Kristian Slavchev",
                    PhoneNumber = "083434343",
                    RegisteredOn = new DateTime(2001, 05, 10)
                },

                new Student
                {
                    Birthday = new DateTime(1887, 06, 25),
                    Name = "Evgeni Pavlov",
                    PhoneNumber = "08923352311",
                    RegisteredOn = new DateTime(2008, 12, 05)
                },

                new Student
                {
                    Birthday = new DateTime(1997, 07, 23),
                    Name = "Desislava Slavcheva",
                    PhoneNumber = "0895220821",
                    RegisteredOn = new DateTime(2015, 08, 21)
                }
            };

            dbContext.Students.AddRange(students);

            var courses = new[]
            {
                new Course
                {
                    Name = "Math",
                    Description = "For beginners",
                    StartDate = new DateTime(1999, 7, 17),
                    EndDate = new DateTime(2016, 10, 21),
                    Price = 190.00m
                },

                new Course
                {
                    Name = "C# Advanced",
                    Description = "Advanced course",
                    StartDate = new DateTime(2007, 10, 17),
                    EndDate = new DateTime(2017, 02, 22),
                    Price = 250.00m
                },

                new Course
                {
                    Name = "Java Fundamentals",
                    Description = "Fundamental concepts",
                    StartDate = new DateTime(2005, 2, 12),
                    EndDate = new DateTime(2005, 12, 25),
                    Price = 190.00m
                }
            };

            dbContext.Courses.AddRange(courses);

            var resources = new[]
            {
                new Resource
                {
                    Name = "Math Intro",
                    Url = "softuni.bg/resources",
                    ResourceType = ResourceType.Presentation,
                    Course = courses[0]
                },

                new Resource
                {
                    Name = "OOP Advanced",
                    Url = "softuni.bg/resources",
                    ResourceType = ResourceType.Document,
                    Course = courses[1]
                },

                new Resource
                {
                    Name = "Stream",
                    Url = "softuni.bg/resources",
                    ResourceType = ResourceType.Document,
                    Course = courses[2]
                }
            };

            dbContext.Resources.AddRange(resources);

            var homeworks = new[]
            {
                new Homework
                {
                    Content = "softuni.bg/homeworks",
                    ContentType = ContentType.Application,
                    SubmissionTime = new DateTime(2016, 2, 5, 12, 45, 55),
                    Course = courses[0],
                    Student = students[1]
                },

                new Homework
                {
                    Content = "softuni.bg/homeworks",
                    ContentType = ContentType.Pdf,
                    SubmissionTime = new DateTime(2015, 4, 8, 14, 22, 36),
                    Course = courses[1],
                    Student = students[0]
                },

                new Homework
                {
                    Content = "softuni.bg/homeworks",
                    ContentType = ContentType.Zip,
                    SubmissionTime = new DateTime(2015, 4, 25, 18, 23, 52),
                    Course = courses[2],
                    Student = students[2]
                }
            };

            dbContext.HomeworkSubmissions.AddRange(homeworks);

            var studentcourses = new[]
            {
                new StudentCourse
                {
                    Student = students[0],
                    Course = courses[0]
                },

                new StudentCourse
                {
                    Student = students[1],
                    Course = courses[1]
                },

                new StudentCourse
                {
                    Student = students[2],
                    Course = courses[2]
                }
            };

            dbContext.StudentCourses.AddRange(studentcourses);
            dbContext.SaveChanges();
        }
    }
}
