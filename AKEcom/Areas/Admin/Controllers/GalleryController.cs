using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AKEcom.Areas.Customer.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class GalleryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GalleryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Gallery/Index
        public IActionResult Index(string category = null)
        {
            var galleries = string.IsNullOrEmpty(category)
                ? _unitOfWork.Gallery.GetAll()
                : _unitOfWork.Gallery.GetAll(g => g.Category == category);

            ViewBag.SelectedCategory = category;
            return View(galleries);
        }

        // GET: Gallery/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gallery/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateGalleryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gallery = new Gallery
                {
                    Title = model.Title,
                    Description = model.Description,
                    Category = model.Category
                };

                // Handle image file upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.ImageFile.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    gallery.ImageUrl = "/images/" + model.ImageFile.FileName;
                }

                _unitOfWork.Gallery.Add(gallery);
                _unitOfWork.Save(); // Save changes
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Gallery/Edit/5
        public IActionResult Edit(int id)
        {
            var gallery = _unitOfWork.Gallery.Get(g => g.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            var model = new CreateGalleryViewModel
            {
                Title = gallery.Title,
                Description = gallery.Description,
                Category = gallery.Category
            };

            return View(model);
        }

        // POST: Gallery/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateGalleryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gallery = _unitOfWork.Gallery.Get(g => g.Id == id);
                if (gallery == null)
                {
                    return NotFound();
                }

                gallery.Title = model.Title;
                gallery.Description = model.Description;
                gallery.Category = model.Category;

                // Handle image file upload
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.ImageFile.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    gallery.ImageUrl = "/images/" + model.ImageFile.FileName;
                }

                _unitOfWork.Gallery.Update(gallery);
                _unitOfWork.Save(); // Save changes
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Gallery/Delete/5
        public IActionResult Delete(int id)
        {
            var gallery = _unitOfWork.Gallery.Get(g => g.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            return View(gallery);
        }

        // POST: Gallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var gallery = _unitOfWork.Gallery.Get(g => g.Id == id);
            if (gallery == null)
            {
                return NotFound();
            }

            _unitOfWork.Gallery.Remove(gallery);
            _unitOfWork.Save(); // Save changes
            return RedirectToAction(nameof(Index));
        }
    }
}
