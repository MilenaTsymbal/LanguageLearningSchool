using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface ILessonTaskRepository
    {
        List<LessonTask> GetAll();
        LessonTask GetById(int id);
        List<LessonTask> GetByLessonId(int lessonId);
        bool DeleteRange(List<LessonTask> tasks);
        bool Add(LessonTask task);
        bool Delete(LessonTask task);
        bool Update(LessonTask task);
        bool Save();
    }
}
