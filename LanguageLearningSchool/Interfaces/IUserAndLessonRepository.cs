using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserAndLessonRepository
    {
        List<UserAndLesson> GetAll();
        UserAndLesson GetById(int id);
        bool Add(UserAndLesson userAndLesson);
        bool Delete(UserAndLesson userAndLesson);
        bool Update(UserAndLesson userAndLesson);
        bool Save();
    }
}
