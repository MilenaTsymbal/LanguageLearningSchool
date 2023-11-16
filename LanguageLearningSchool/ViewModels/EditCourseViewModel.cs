using LanguageLearningSchool.Enums;
using LanguageLearningSchool.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace LanguageLearningSchool.ViewModels
{
    public class EditCourseViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string Description { get; set; }
        public Languages Language { get; set; }
        public DifficultyLevels DifficultyLevel { get; set; }
    }
}
