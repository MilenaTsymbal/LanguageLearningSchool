using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class PassTheTestViewModel
    {
        public int LessonId { get; set; }
        public string LessonName { get; set; }
        public List<LessonTask> Tasks { get; set; }
        public List<Answer> Answers { get; set; }
        public List<UserAndTask> UserAndTasks { get; set; }
    }
}
