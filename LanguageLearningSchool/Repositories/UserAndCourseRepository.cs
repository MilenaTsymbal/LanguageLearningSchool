using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LanguageLearningSchool.Repositories
{
    public class UserAndCourseRepository : IUserAndCourseRepository
    {
        public UserAndCourseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool Add(UserAndCourse userAndCourse)
        {
            _context.Add(userAndCourse);
            return Save();
        }

        public bool Delete(UserAndCourse userAndCourse)
        {
            _context.Remove(userAndCourse);
            return Save();
        }

        public List<UserAndCourse> GetAll()
        {
            return _context.UsersAndCourses.Include(uc => uc.Course).Include(uc => uc.User).ToList();
        }

        public UserAndCourse? GetById(int id)
        {
            return _context.UsersAndCourses.FirstOrDefault(c => c.UserAndCourseId == id);
        }

        public UserAndCourse FirstOrDefault(Expression<Func<UserAndCourse, bool>> predicate)
        {
            return _context.UsersAndCourses.FirstOrDefault(predicate);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(UserAndCourse userAndCourse)
        {
            _context.Update(userAndCourse);
            return Save();
        }
    }
}
