using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserAndLessonRepository
    {
        List<UserAndLesson> GetAll();
        UserAndLesson GetById(int id);
        List<UserAndLesson> GetUserAndLessonsForCourse(int courseId);
        IQueryable<UserAndLesson> GetAllLessonsUserLearning();
        bool DeleteRange(List<UserAndLesson> userAndLesson);
        bool Add(UserAndLesson userAndLesson);
        bool Delete(UserAndLesson userAndLesson);
        bool Update(UserAndLesson userAndLesson);
        bool Save();
    }
}
