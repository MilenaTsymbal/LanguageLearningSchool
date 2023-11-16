using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class CourseIndexViewModel
    {
        public List<Course> Courses { get; set; }
        public List<UserAndCourse> UserAndCourses { get; set; }
    }
}
