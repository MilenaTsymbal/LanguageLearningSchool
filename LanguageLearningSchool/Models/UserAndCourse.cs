using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LanguageLearningSchool.Models
{
    public class UserAndCourse
    {
        [Key]  
        public int UserAndCourseId { get; set; }

        [ForeignKey ("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public Course Course { get; set; }
        public DateTime? DateOfCreation { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? GeneralEstimation { get; set; }
    }
}
