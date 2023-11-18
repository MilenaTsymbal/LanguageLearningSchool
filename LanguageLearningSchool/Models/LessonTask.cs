using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.Models
{
    public class LessonTask
    {
        [Key]
        public int TaskId { get; set; }
        [ForeignKey("Lesson")]
        public int LessonId { get; set; }
        public string TaskText { get; set; }
    }
}
