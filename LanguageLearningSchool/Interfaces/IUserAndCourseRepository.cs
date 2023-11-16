using LanguageLearningSchool.Models;
using System.Linq.Expressions;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserAndCourseRepository
    {
        List<UserAndCourse> GetAll();
        UserAndCourse? GetById(int id);
        bool Add(UserAndCourse userAndCourse);
        bool Delete(UserAndCourse userAndCourse);
        public UserAndCourse FirstOrDefault(Expression<Func<UserAndCourse, bool>> predicate);
        bool Update(UserAndCourse userAndCourse);
        bool Save();
    }
}
