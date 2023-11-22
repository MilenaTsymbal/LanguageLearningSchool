using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class EditTaskViewModel
    {
        public LessonTask Task { get; set; }
        public List<Answer> Answers { get; set; }
        public string SelectedCorrectAnswer { get; set; }
    }
}
