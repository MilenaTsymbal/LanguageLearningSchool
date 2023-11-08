using LanguageLearningSchool.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LanguageLearningSchool.Models
{
    public class Course
    {
        [Key]
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public Languages Language { get; set; }
        public DifficultyLevels DifficultyLevel { get; set; }
    }
}
