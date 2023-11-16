using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class CourseDetailsViewModel
    {
        public Course Course { get; set; }
        //public List<User> Users { get; set; }
        public List<UserAndCourse> UsersAndCourse { get; set; }
    }
}
