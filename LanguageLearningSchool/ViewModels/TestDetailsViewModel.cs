using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class TestDetailsViewModel
    {
        public string LessonName { get; set; }
        public List<LessonTask> Tasks { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
