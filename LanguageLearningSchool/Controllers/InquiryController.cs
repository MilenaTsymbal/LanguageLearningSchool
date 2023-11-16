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
        public ActionResult RunInquiry(string inquiryText)
        {
            var model = new InquiryViewModel
            {
                InquiryText = inquiryText
            };

            try
            {
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
