using LanguageLearningSchool.Enums;
using LanguageLearningSchool.Models;

namespace LanguageLearningSchool.ViewModels
{
    public class CourseIndexViewModel
    {
        public List<Course> Courses { get; set; }
        public List<UserAndCourse> UserAndCourses { get; set; }
        public List<UserAndCourse> UserCreatorsOfCourses { get; set; }
        public Languages? LanguageFilter { get; set; }
        public DifficultyLevels? DifficultyLevelFilter { get; set; }
        public string? AlphabeticalOrder { get; set; }
        public string? DateOfCreationOrder { get; set; }
        public string? SearchQuery { get; set; }
    }
}
