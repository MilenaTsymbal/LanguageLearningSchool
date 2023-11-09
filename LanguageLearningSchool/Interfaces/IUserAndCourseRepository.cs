using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserAndCourseRepository
    {
        List<UserAndCourse> GetAll();
        UserAndCourse? GetById(int id);
        bool Add(UserAndCourse userAndCourse);
        bool Delete(UserAndCourse userAndCourse);
        bool Update(UserAndCourse userAndCourse);
        bool Save();
    }
}
