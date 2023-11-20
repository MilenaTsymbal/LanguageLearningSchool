using LanguageLearningSchool.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.ViewModels
{
    public class CreateLessonViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        [Required(ErrorMessage = "Lesson name is required")]
        public string LessonName { get; set; }
        [Required(ErrorMessage = "Material is required")]
        public string Material { get; set; }
        public bool IsTheLastLesson { get; set; }
    }
}
