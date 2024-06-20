using AK.DataAccess.Repository;
using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Models.ViewModels;
using AK.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe.Checkout;
using System.Security.Claims;
using System.Security.Permissions;

namespace AKEcom.Areas.Customer.Controllers 
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitofwork;
        [BindProperty]
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
                UserId, includeproperties: "Product"),
                OrderHeader=new()
            };

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }



        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeproperties: "Product"),
                OrderHeader = new()
            };

            ShoppingCartVM.OrderHeader.ApplicationUser = _unitofwork.ApplicationUser.Get(u => u.Id == userId);

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;



            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = GetPriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }
            return View(ShoppingCartVM);
           
        }







        [HttpPost]
        [ActionName("Summary")]
		public IActionResult SummaryPOST(ShoppingCartVM shoppingcartVM)
		{
			var claimsIdentity = (ClaimsIdentity)User.Identity;
			var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitofwork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeproperties: "Product");

            ShoppingCartVM.OrderHeader.OrderDate = System.DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

			ApplicationUser applicationUser = _unitofwork.ApplicationUser.Get(u => u.Id == userId);

			



			foreach (var cart in ShoppingCartVM.ShoppingCartList)
			{
				cart.Price = GetPriceBasedOnQuantity(cart);
				ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
			}

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusPending;

			}
            else
            {
				ShoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusDelayedPayment;
				ShoppingCartVM.OrderHeader.OrderStatus = SD.StatusApproved;
			}
            _unitofwork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitofwork.Save();
            foreach (var cart in  ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderdetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitofwork.OrderDetail.Add(orderdetail);
                _unitofwork.Save();
            }
            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                //stripe logic
                var domain = "http://localhost:5092/";
				var options = new Stripe.Checkout.SessionCreateOptions
				{
					SuccessUrl = domain+ $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain+"customer/cart/index",
					LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
	              
					  Mode = "payment",
				};

                foreach(var item in ShoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title

                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }


				var service = new Stripe.Checkout.SessionService();
				Session session= service.Create(options);
                _unitofwork.OrderHeader.UpdateStripePaymentId(ShoppingCartVM.OrderHeader.Id, session.Id, session.PaymentIntentId);
                _unitofwork.Save();
                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
			}

			return RedirectToAction(nameof(OrderConfirmation),new {id=ShoppingCartVM.OrderHeader.Id});

		}

		public IActionResult OrderConfirmation(int id)
		{
			OrderHeader orderHeader = _unitofwork.OrderHeader.Get(u => u.Id == id, includeproperties: "ApplicationUser");
			if (orderHeader == null)
			{
				return NotFound();
			}

			if (orderHeader.PaymentStatus != SD.PaymentStatusDelayedPayment)
			{
				var service = new SessionService();
				Session session = service.Get(orderHeader.SessionId);

				if (session.PaymentStatus.ToLower() == "paid")
				{
					_unitofwork.OrderHeader.UpdateStripePaymentId(id, session.Id, session.PaymentIntentId);
					_unitofwork.OrderHeader.UpdateStatus(id, SD.StatusApproved, SD.PaymentStatusApproved);
					_unitofwork.Save();
				}
			}

			List<ShoppingCart> shoppingCarts = _unitofwork.ShoppingCart
				.GetAll(u => u.ApplicationUserId == orderHeader.ApplicationUserId).ToList();

			_unitofwork.ShoppingCart.RemoveRange(shoppingCarts);
			_unitofwork.Save();

			return View(id);
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
