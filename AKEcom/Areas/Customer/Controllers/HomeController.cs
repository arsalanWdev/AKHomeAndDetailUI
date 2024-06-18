using AK.DataAccess.Repository.IRepository;
using AK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AKEcom.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
       
        private readonly IUnitOfWork _unitofwork;

        public HomeController(ILogger<HomeController> logger,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitofwork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productlist = _unitofwork.Product.GetAll(includeproperties:"Category");
            return View(productlist);
        }

        public IActionResult Detail(int productId)
        {
            Product product = _unitofwork.Product.Get(u => u.Id == productId,includeproperties: "Category");
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
