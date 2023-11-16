using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Repositories
{
    public class LessonRepository : ILessonRepository
    {
        public LessonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool Add(Lesson lesson)
        {
            _context.Add(lesson);
            return Save();
        }

        public bool Delete(Lesson lesson)
        {
            _context.Remove(lesson);
            return Save();
        }

        public List<Lesson> GetAll()
        {
            return _context.Lessons.ToList();
        }

        public Lesson GetById(int id)
        {
            return _context.Lessons.FirstOrDefault(c => c.LessonId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Lesson lesson)
        {
            _context.Update(lesson);
            return Save();
        }

    }
}
