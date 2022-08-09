using Microsoft.AspNetCore.Mvc;

namespace permissions.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
