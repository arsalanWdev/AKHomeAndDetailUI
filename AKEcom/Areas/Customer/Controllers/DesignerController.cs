using Microsoft.AspNetCore.Mvc;

namespace AKEcom.Areas.Customer.Controllers
{
    public class DesignerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
