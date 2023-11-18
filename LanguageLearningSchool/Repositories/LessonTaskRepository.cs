using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Repositories
{
    public class LessonTaskRepository : ILessonTaskRepository
    {
        public LessonTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool Add(LessonTask task)
        {
            _context.Add(task);
            return Save();
        }

        public bool Delete(LessonTask task)
        {
            _context.Remove(task);
            return Save();
        }

        public List<LessonTask> GetAll()
        {
            return _context.LessonTasks.ToList();
        }

        public LessonTask GetById(int id)
        {
            return _context.LessonTasks.FirstOrDefault(c => c.TaskId == id);
        }

        public List<LessonTask> GetByLessonId(int lessonId)
        {
            return _context.LessonTasks.Where(c => c.LessonId == lessonId).ToList();
        }

        public bool DeleteRange(List<LessonTask> tasks)
        {
            _context.Set<LessonTask>().RemoveRange(tasks);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(LessonTask task)
        {
            _context.Update(task);
            return Save();
        }
    }
}
