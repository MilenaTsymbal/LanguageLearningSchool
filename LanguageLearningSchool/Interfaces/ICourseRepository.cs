using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface ICourseRepository
    {
        List<Course> GetAll();
        Course GetById(int id);
        bool Add(Course course);
        bool Delete(Course course);
        bool Update(Course course);
        bool Save();
    }
}
