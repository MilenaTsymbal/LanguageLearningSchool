using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class CourseDetailsViewModel
    {
        public Course Course { get; set; }
        public List<UserAndCourse> UsersAndCourse { get; set; }
        public List<Lesson> Lessons { get; set; }
        public List<UserAndLesson> UserAndLessons { get; set; }
    }
}
