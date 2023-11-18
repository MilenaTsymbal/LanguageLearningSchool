using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.Models
{
    public class Answer
    {
        public int AnswerId { get; set; }
        [ForeignKey("LessonTask")]
        public int TaskId { get; set; }
        public string AnswerName { get; set; }
        public bool Correctness { get; set; }

    }
}
