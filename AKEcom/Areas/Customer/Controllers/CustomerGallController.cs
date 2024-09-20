using AK.DataAccess.Repository.IRepository;
using AK.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AKEcom.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize(Roles ="Customer")] // Ensure the user is logged in to manage favourites
    public class CustomerGallController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CustomerGallController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        // This method will run before every action and set the favourite count in the ViewBag
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!string.IsNullOrEmpty(userId))
            {
                // Get the count of favorite items for the logged-in user
                var favouriteCount = _unitOfWork.Favourite.GetAll(f => f.UserId == userId).Count();
                ViewBag.FavouriteCount = favouriteCount; // Pass the count to the view
            }
            else
            {
                ViewBag.FavouriteCount = 0; // No user logged in, no favourites
            }

            base.OnActionExecuting(context); // Ensure the base method is called
        }


        // GET: /Customer/CustomerGall/Index
        public async Task<IActionResult> Index()
        {
            var galleries = await _unitOfWork.Gallery.GetAllAsync(); // Get all gallery images
            return View(galleries); // Pass them to the view
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToFavourites(int galleryId)
        {
            // Get the logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // User is not logged in
            }

            // Check if the gallery item is already in the user's favourites
            var existingFavourite = await _unitOfWork.Favourite.GetFirstOrDefaultAsync(f => f.UserId == userId && f.GalleryId == galleryId);

            if (existingFavourite == null)
            {
                // Create new favourite
                var favourite = new Favourite
                {
                    UserId = userId,
                    GalleryId = galleryId
                };

                // Add the item to the database
                _unitOfWork.Favourite.Add(favourite);

                // Save changes
                try
                {
                    _unitOfWork.Save();
                }
                catch (DbUpdateException ex)
                {
                    // Log the error or handle it
                    return BadRequest("Unable to add to favourites: " + ex.Message);
                }
            }
            else
            {
                // Handle case where item is already in favourites, if needed
                return BadRequest("This item is already in your favourites.");
            }
            TempData["success"] = "Add To Favourites Successfully";

            return RedirectToAction(nameof(MyFavourites));
        }



        // GET: /Customer/CustomerGall/MyFavourites
        public async Task<IActionResult> MyFavourites()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get user ID from claims

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Ensure the user is logged in
            }

            var favourites = await _unitOfWork.Favourite.GetFavouritesByUserAsync(userId);

            return View(favourites); // Pass the favorites to the view
        }


        // POST: /Customer/CustomerGall/RemoveFromFavourites
        [HttpPost]
        public async Task<IActionResult> RemoveFromFavourites(int galleryId)
        {
            // Get the currently logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(); // Ensure the user is logged in
            }

            // Find the favourite entry using a predicate
            var favourite = await _unitOfWork.Favourite.GetFirstOrDefaultAsync(f => f.UserId == userId && f.GalleryId == galleryId);

            if (favourite != null)
            {
                _unitOfWork.Favourite.Remove(favourite); // Remove from favourites
                _unitOfWork.Save(); // Save changes
            }

            return RedirectToAction(nameof(MyFavourites)); // Redirect back to the favourites page
        }

    }
}
