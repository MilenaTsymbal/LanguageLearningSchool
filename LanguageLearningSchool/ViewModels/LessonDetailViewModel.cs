using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class LessonDetailViewModel
    {
        public Lesson Lesson { get; set; }
        public Course Course { get; set; }
        public List<UserAndCourse> UsersAndCourse { get; set; }
        public UserAndLesson UserAndLesson { get; set; }
        public List<LessonTask> Tasks { get; set; }
    }
}
