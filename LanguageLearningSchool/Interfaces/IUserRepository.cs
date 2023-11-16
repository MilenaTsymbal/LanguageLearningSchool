using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserRepository
    {
        List<User> GetAll();
        User GetById(string id);
        bool Add(User user);
        bool Delete(User user);
        bool Update(User user);
        bool Save();
    }
}
