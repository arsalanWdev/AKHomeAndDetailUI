using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

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
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart,
                   _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count());
            }
            IEnumerable<Product> productlist = _unitofwork.Product.GetAll(includeproperties:"Category");
            return View(productlist);
        }

        public IActionResult Detail(int productId)
        {
            ShoppingCart cart = new()
            {
                Product = _unitofwork.Product.Get(u => u.Id == productId, includeproperties: "Category"),
                Count = 1,
                ProductId = productId

            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Detail(ShoppingCart shoppingcart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            shoppingcart.ApplicationUserId = UserId;


            ShoppingCart cartfromdb = _unitofwork.ShoppingCart.Get(u => u.ApplicationUserId == UserId &&
            u.ProductId == shoppingcart.ProductId);

            if(cartfromdb != null)
            {
                cartfromdb.Count += shoppingcart.Count;
                _unitofwork.ShoppingCart.Update(cartfromdb);
                _unitofwork.Save();

            }
            else
            {
                _unitofwork.ShoppingCart.Add(shoppingcart);
                _unitofwork.Save();

                HttpContext.Session.SetInt32(SD.SessionCart, 
                    _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == UserId).Count());

            }
            TempData["success"] = "Cart Updated Successfully";


            return RedirectToAction(nameof(Index));
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
