using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface ILessonTaskRepository
    {
        List<LessonTask> GetAll();
        LessonTask GetById(int id);
        bool Add(LessonTask task);
        bool Delete(LessonTask task);
        bool Update(LessonTask task);
        bool Save();
    }
}
