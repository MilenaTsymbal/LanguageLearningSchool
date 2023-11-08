using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface ICourseRepository
    {
        Task<IEnumerable<Course>> GetAll();
        Task<Course> GetByIdAsync(int id);
        bool Add(Course course);
        bool Delete(Course course);
        bool Update(Course course);
        bool Save();
    }
}
