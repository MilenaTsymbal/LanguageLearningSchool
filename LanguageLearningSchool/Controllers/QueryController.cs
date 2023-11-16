using Microsoft.AspNetCore.Mvc;

namespace LanguageLearningSchool.Controllers
{
    public class QueryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
