using LanguageLearningSchool.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace LanguageLearningSchool.Models
{
    public class User : IdentityUser
    {
        public Roles UserRole { get; set; }
        public string Name { get; set; }
        public string MiddleName { get; set; }
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
    }
}
