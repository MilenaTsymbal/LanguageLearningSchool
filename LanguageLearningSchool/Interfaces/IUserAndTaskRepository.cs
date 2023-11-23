using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Interfaces
{
    public interface IUserAndTaskRepository
    {
        List<UserAndTask> GetAll();
        UserAndTask GetById(int id);
        List<UserAndTask> GetAllUsersOnTask(int taskId);
        bool AddRange(List<UserAndTask> userAndTask);
        bool UpdateRange(List<UserAndTask> userAndTask);
        bool DeleteRange(List<UserAndTask> userAndTask);
        bool Add(UserAndTask userAndTask);
        bool Delete(UserAndTask userAndTask);
        bool Update(UserAndTask userAndTask);
        bool Save();
    }
}
