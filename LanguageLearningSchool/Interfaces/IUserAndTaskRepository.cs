using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserAndTaskRepository
    {
        List<UserAndTask> GetAll();
        UserAndTask GetById(int id);
        bool Add(UserAndTask userAndTask);
        bool Delete(UserAndTask userAndTask);
        bool Update(UserAndTask userAndTask);
        bool Save();
    }
}
