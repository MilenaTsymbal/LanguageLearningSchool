using LanguageLearningSchool.Data;
using LanguageLearningSchool.Interfaces;
using LanguageLearningSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace LanguageLearningSchool.Repositories
{
    public class CourseRepository : ICourseRepository
    {

        public CourseRepository(ApplicationDbContext context) 
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool Add(Course course)
        {
            _context.Add(course);
            return Save();
        }

        public bool Delete(Course course)
        {
            _context.Remove(course);
            return Save();
        }

        public List<Course> GetAll()
        {
            return _context.Courses.ToList();
        }

        public Course GetById(int id)
        {
            return _context.Courses.FirstOrDefault(c => c.CourseId == id);
        }
        
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Course course)
        {
            _context.Update(course);
            return Save();
        }
    }
}
