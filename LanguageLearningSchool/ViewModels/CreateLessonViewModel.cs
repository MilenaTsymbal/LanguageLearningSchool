using LanguageLearningSchool.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.ViewModels
{
    public class CreateLessonViewModel
    {
        public int CourseId { get; set; }
        [Required(ErrorMessage = "Lesson name is required")]
        public string LessonName { get; set; }
        [Required(ErrorMessage = "Material is required")]
        public string Material { get; set; }
    }
}
