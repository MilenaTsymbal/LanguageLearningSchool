using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.Models
{
    public class Lesson
    {
        [Key]
        public int LessonId { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public string Name { get; set; }
        public string Material { get; set; }
        public int? LessonEstimation { get; set; }
    }
}
