using LanguageLearningSchool.Models;
using System.ComponentModel.DataAnnotations;

namespace LanguageLearningSchool.ViewModels
{
    public class CreateLessonTaskViewModel
    {
        public int LessonId { get; set; }
        [Required(ErrorMessage = "Task text is required")]
        public string TaskText { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
