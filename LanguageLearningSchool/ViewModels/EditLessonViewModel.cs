using LanguageLearningSchool.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.ViewModels
{
    public class EditLessonViewModel
    {
        public int LessonId { get; set; }
        public int CourseId { get; set; }
        public string LessonName { get; set; }
        public string Material { get; set; }
        public bool IsTheLastLesson { get; set; }
    }
}
