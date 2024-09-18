using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace AKEcom.Areas.Customer.Controllers
{
    [Area("Customer")]



    [Authorize(Roles = "InteriorDesigner")]
    public class PortfolioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _userId;

        public PortfolioController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userId = httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // GET: Portfolio
        public IActionResult Index()
        {
            var portfolios = _unitOfWork.Portfolio.GetAll(p => p.UserId == _userId);
            return View(portfolios);
        }

        // GET: Portfolio/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Portfolio/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreatePortfolioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var portfolio = new Portfolio
                {
                    Title = model.Title,
                    Description = model.Description,
                    UserId = _userId // Set the user ID
                };

                // Handle image file upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.ImageFile.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    portfolio.ImageUrl = "/images/" + model.ImageFile.FileName;
                }

                _unitOfWork.Portfolio.Add(portfolio);
                _unitOfWork.Save(); // Save changes using your Save method
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Portfolio/Edit/5
        public IActionResult Edit(int id)
        {
            var portfolio = _unitOfWork.Portfolio.Get(p => p.Id == id && p.UserId == _userId);
            if (portfolio == null)
            {
                return NotFound();
            }

            var model = new CreatePortfolioViewModel
            {
                Title = portfolio.Title,
                Description = portfolio.Description,
                // For existing image URL, you can include it in the ViewModel if needed
            };

            return View(model);
        }

        // POST: Portfolio/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreatePortfolioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var portfolio = _unitOfWork.Portfolio.Get(p => p.Id == id && p.UserId == _userId);
                if (portfolio == null)
                {
                    return NotFound();
                }

                portfolio.Title = model.Title;
                portfolio.Description = model.Description;

                // Handle image file upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.ImageFile.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    portfolio.ImageUrl = "/images/" + model.ImageFile.FileName;
                }

                _unitOfWork.Portfolio.Update(portfolio);
                _unitOfWork.Save(); // Save changes using your Save method
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Portfolio/Delete/5
        public IActionResult Delete(int id)
        {
            var portfolio = _unitOfWork.Portfolio.Get(p => p.Id == id && p.UserId == _userId);
            if (portfolio == null)
            {
                return NotFound();
            }

            return View(portfolio);
        }
        [HttpPost, ActionName("DeleteConfirmed")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var portfolio = _unitOfWork.Portfolio.Get(p => p.Id == id && p.UserId == _userId);
            if (portfolio == null)
            {
                return NotFound();
            }

            _unitOfWork.Portfolio.Remove(portfolio);
            _unitOfWork.Save(); // Call the synchronous Save method
            return RedirectToAction(nameof(Index));
        }


    }

}