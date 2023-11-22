using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;
using System.Data;
using System;
using LanguageLearningSchool.ViewModels;

namespace LanguageLearningSchool.Controllers
{
    public class InquiryController : Controller
    {
        public IActionResult Index()
        {
            var model = new InquiryViewModel();
            return View(model);
        }

        [HttpPost]
        public ActionResult RunInquiry(string inquiryText, string specificQuery)
        {
            var model = new InquiryViewModel
            {
                InquiryText = inquiryText,
                SpecificQuery = specificQuery
            };

            try
            {
                if (specificQuery == "StudentsRegisteredOnSpecificCourse")
                {
                    model.InquiryText = "DECLARE @courseId INT = 1;\r\n\r\nSELECT\r\n    u.Name AS FirstName,\r\n    u.MiddleName,\r\n    u.Surname AS LastName,\r\n    COALESCE(uc.GeneralEstimation, '-') AS CourseEstimation,\r\n    COUNT(ul.LessonId) AS CompletedLessons,\r\n    ROUND(AVG(CAST(ul.LessonEstimation AS FLOAT)), 2) AS AverageLessonEstimation\r\nFROM AspNetUsers u\r\nJOIN UsersAndCourses uc ON u.Id = uc.UserId\r\nJOIN Courses c ON uc.CourseId = c.CourseId\r\nLEFT JOIN UsersAndLessons ul ON u.Id = ul.UserId\r\nLEFT JOIN Lessons l ON ul.LessonId = l.LessonId\r\nWHERE c.CourseId = @courseId\r\nGROUP BY u.Id, u.Name, u.MiddleName, u.Surname, uc.GeneralEstimation;";
                }
                else if (specificQuery == "AverageDaysSpentOnCourse")
                {
                    model.InquiryText = "SELECT c.CourseId, c.CourseName, AVG(DATEDIFF(day, uc.StartDate, uc.EndDate)) AS AverageDaysToComplete\r\nFROM Courses c\r\nLEFT JOIN UsersAndCourses uc ON c.CourseId = uc.CourseId\r\nGROUP BY c.CourseId, c.CourseName;";
                }
                else if (specificQuery == "AvailableCourse")
                {
                    model.InquiryText = "SELECT ROW_NUMBER() OVER (ORDER BY CourseId) AS CourseIndex,\r\n       CourseName,\r\n       Description,\r\n       CASE Language\r\n           WHEN 0 THEN 'English'\r\n           WHEN 1 THEN 'Spanish'\r\n           WHEN 2 THEN 'French'\r\n           WHEN 3 THEN 'German'\r\n           WHEN 4 THEN 'Chinese'\r\n           WHEN 5 THEN 'Japanese'\r\n           WHEN 6 THEN 'Italian'\r\n           WHEN 7 THEN 'Portuguese'\r\n           WHEN 8 THEN 'Korean'\r\n       END AS Language,\r\n       CASE DifficultyLevel\r\n           WHEN 0 THEN 'Elementary'\r\n           WHEN 1 THEN 'PreIntermediate'\r\n           WHEN 2 THEN 'Intermediate'\r\n           WHEN 3 THEN 'UpperIntermediate'\r\n           WHEN 4 THEN 'Advanced'\r\n           WHEN 5 THEN 'Proficient'\r\n       END AS DifficultyLevel\r\nFROM Courses;";
                }
                else if (specificQuery == "GetTop3Courses")
                {
                    model.InquiryText = "SELECT TOP 3\r\n    c.CourseId,\r\n    c.CourseName,\r\n    COUNT(uc.UserId) AS RegisteredUsers\r\nFROM Courses c\r\nLEFT JOIN UsersAndCourses uc ON c.CourseId = uc.CourseId\r\nGROUP BY c.CourseId, c.CourseName\r\nORDER BY RegisteredUsers DESC;";
                }

                const string connectionString = "Data Source=DESKTOP-2G8IMH5;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False;Database=School;";
                using var connection = new SqlConnection(connectionString);
                using var command = new SqlCommand(inquiryText, connection);
                connection.Open();
                var reader = command.ExecuteReader();
                var table = new DataTable();
                table.Load(reader);
                model.Output = table;
            }
            catch (Exception ex)
            {
                model.Output = null;
            }
            return View("Index", model);
        }
    }

}
