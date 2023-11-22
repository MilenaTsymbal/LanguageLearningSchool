using System.Data;

namespace LanguageLearningSchool.ViewModels
{
    public class InquiryViewModel
    {
        public string? InquiryText { get; set; }
        public DataTable? Output { get; set; }
        public string? SpecificQuery { get; set; }
    }
}
