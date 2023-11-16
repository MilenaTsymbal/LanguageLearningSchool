using LanguageLearningSchool.Enums;

namespace LanguageLearningSchool.ViewModels
{
    public class EditUserViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Roles UserRole { get; set; }
    }
}
