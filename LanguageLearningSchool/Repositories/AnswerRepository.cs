using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        public AnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool Add(Answer answer)
        {
            _context.Add(answer);
            return Save();
        }

        public bool Delete(Answer answer)
        {
            _context.Remove(answer);
            return Save();
        }

        public List<Answer> GetAll()
        {
            return _context.Answers.ToList();
        }

        public List<Answer> GetAllAnswersOfLesson(int lessonId)
        {
            return _context.Answers.Where(a => _context.LessonTasks.Any(lt => lt.LessonId == lessonId && lt.TaskId == a.TaskId))
            .ToList();
        }

        public List<Answer> GetAllAnswersOfTask(int taskId)
        {
            return _context.Answers.Where(a => a.TaskId == taskId).ToList();
        }

        public Answer GetById(int id)
        {
            return _context.Answers.FirstOrDefault(c => c.TaskId == id);
        }

        public bool UpdateRange(List<Answer> answers)
        {
            _context.Set<Answer>().UpdateRange(answers);
            return Save();
        }

        public bool AddRange(List<Answer> answers)
        {
            _context.Set<Answer>().AddRange(answers);
            return Save();
        }

        public bool DeleteRange(List<Answer> answers)
        {
            _context.Set<Answer>().RemoveRange(answers);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Answer answer)
        {
            _context.Update(answer);
            return Save();
        }
    }
}
