using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Repositories
{
    public class UserAndLessonRepository : IUserAndLessonRepository
    {
        public UserAndLessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool Add(UserAndLesson userAndLesson)
        {
            _context.Add(userAndLesson);
            return Save();
        }

        public bool Delete(UserAndLesson userAndLesson)
        {
            _context.Remove(userAndLesson);
            return Save();
        }

        public List<UserAndLesson> GetAll()
        {
            return _context.UsersAndLessons.ToList();
        }

        public UserAndLesson GetById(int id)
        {
            return _context.UsersAndLessons.FirstOrDefault(c => c.UserAndLessonId == id);
        }

        public List<UserAndLesson> GetUserAndLessonsForCourse(int courseId)
        {
            return _context.UsersAndLessons.Include(ual => ual.User).Include(ual => ual.Lesson)
                .Where(ual => ual.Lesson.CourseId == courseId).ToList();
        }

        public bool DeleteRange(List<UserAndLesson> userAndLesson)
        {
            _context.Set<UserAndLesson>().RemoveRange(userAndLesson);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(UserAndLesson userAndLesson)
        {
            _context.Update(userAndLesson);
            return Save();
        }
    }
}
