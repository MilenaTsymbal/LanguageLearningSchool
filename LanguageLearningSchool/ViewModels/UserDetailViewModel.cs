using LanguageLearningSchool.Enums;
using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class UserDetailViewModel
    {
        public User User { get; set; }
        public List<UserAndCourse> UserAndCourses { get; set; }
    }
}
