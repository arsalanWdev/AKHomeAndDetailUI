using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AKEcom.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitofwork)
        {
            _unitofwork = unitofwork;
        }


        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId ==
                UserId, includeproperties: "Product")
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }



        public IActionResult Summary()
        {
            return View();
        }


        public IActionResult Plus(int cartId)
        {
            var cartfromdb = _unitofwork.ShoppingCart.Get(u => u.Id == cartId);

            cartfromdb.Count += 1;
            _unitofwork.ShoppingCart.Update(cartfromdb);
            _unitofwork.Save();

            return RedirectToAction(nameof(Index));

        }


        public IActionResult Minus(int cartId)
        {
            var cartfromdb = _unitofwork.ShoppingCart.Get(u => u.Id == cartId);
            if (cartfromdb.Count <= 1)
            {
                _unitofwork.ShoppingCart.Remove(cartfromdb);
            }
            else
            {
                cartfromdb.Count -= 1;
                _unitofwork.ShoppingCart.Update(cartfromdb);
            }
            _unitofwork.Save();

            return RedirectToAction(nameof(Index));

        }


        public IActionResult Remove(int cartId)
        {
            var cartfromdb = _unitofwork.ShoppingCart.Get(u => u.Id == cartId);

            _unitofwork.ShoppingCart.Remove(cartfromdb);
            _unitofwork.Save();

            return RedirectToAction(nameof(Index));

        }
















        private double GetPriceBasedOnQuantity(ShoppingCart shoppingcart)
        {
            if (shoppingcart.Count <= 50)
            {
                return shoppingcart.Product.Price;
            }
            else
            {
                if (shoppingcart.Count <= 100)
                {
                    return shoppingcart.Product.Price50;
                }
                else
                {
                    return shoppingcart.Product.Price100;
                }
            }
        }
    }
}
