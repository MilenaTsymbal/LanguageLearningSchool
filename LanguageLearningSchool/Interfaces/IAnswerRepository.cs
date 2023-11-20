using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IAnswerRepository
    {
        List<Answer> GetAllAnswersOfLesson(int lessonId);
        List<Answer> GetAllAnswersOfTask(int taskId);
        bool UpdateRange(List<Answer> answers);
        bool AddRange(List<Answer> answers);
        bool DeleteRange(List<Answer> answers);
        Answer GetById(int id);
        bool Add(Answer course);
        bool Delete(Answer course);
        bool Update(Answer course);
        bool Save();
    }
}
