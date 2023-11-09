using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Repositories
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User? GetByIdAsync(int id)
        {
            string idAsString = id.ToString();
            return _context.Users.FirstOrDefault(c => c.Id == idAsString);
        }

        public bool Add(User user)
        {
            _context.Add(user);
            return Save();
        }

        public bool Delete(User user)
        {
            _context.Remove(user);
            return Save();
        }

        public bool Update(User user)
        {
            _context.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
