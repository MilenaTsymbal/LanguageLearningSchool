using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAll();
        Task<User> GetByIdAsync(int id);
        bool Add(User user);
        bool Delete(User user);
        bool Update(User user);
        bool Save();
    }
}
