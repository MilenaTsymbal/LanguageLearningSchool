using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class LessonDetailViewModel
    {
        public Lesson Lesson { get; set; }
        public Course Course { get; set; }
        public List<UserAndCourse> UsersAndCourse { get; set; }
    }
}
