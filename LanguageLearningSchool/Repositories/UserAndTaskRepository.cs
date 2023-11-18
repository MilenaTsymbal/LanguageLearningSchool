using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.Repositories
{
    public class UserAndTaskRepository : IUserAndTaskRepository
    {
        public UserAndTaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool Add(UserAndTask userAndTask)
        {
            _context.Add(userAndTask);
            return Save();
        }

        public bool Delete(UserAndTask userAndTask)
        {
            _context.Remove(userAndTask);
            return Save();
        }

        public List<UserAndTask> GetAll()
        {
            return _context.UsersAndTasks.ToList();
        }

        public UserAndTask GetById(int id)
        {
            return _context.UsersAndTasks.FirstOrDefault(c => c.UserAndTaskId == id);
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(UserAndTask userAndTask)
        {
            _context.Update(userAndTask);
            return Save();
        }
    }
}
