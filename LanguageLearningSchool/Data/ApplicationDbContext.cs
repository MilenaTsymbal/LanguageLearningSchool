using LanguageLearningSchool.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AutoSchema = true;
        }
        public bool AutoSchema { get; }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserAndCourse> UsersAndCourses { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
    }
}
