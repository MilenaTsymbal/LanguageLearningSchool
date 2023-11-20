using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.Models
{
    public class UserAndTask
    {
        [Key]
        public int UserAndTaskId { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        [ForeignKey("LessonTask")]
        public int TaskId { get; set; }
        public LessonTask Task { get; set; }
        public bool UserAnswer { get; set; }
    }
}
