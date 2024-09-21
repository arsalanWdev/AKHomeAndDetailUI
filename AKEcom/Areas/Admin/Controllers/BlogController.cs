using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace AKEcom.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BlogController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Blog/Index
        public IActionResult Index()
        {
            var blogPosts = _unitOfWork.BlogPost.GetAll();
            return View(blogPosts);
        }

        // GET: Blog/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var blogPost = new BlogPost
                {
                    Title = model.Title,
                    Content = model.Content
                };

                // Handle image file upload
                if (model.photo != null && model.photo.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.photo.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.photo.CopyToAsync(fileStream);
                    }

                    blogPost.ImagePath = "/images/" + model.photo.FileName;
                }

                _unitOfWork.BlogPost.Add(blogPost);
                _unitOfWork.Save(); // Save changes
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Blog/Edit/5
        public IActionResult Edit(int id)
        {
            var blogPost = _unitOfWork.BlogPost.Get(b => b.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            var model = new BlogPostViewModel
            {
                Id = blogPost.Id,
                Title = blogPost.Title,
                Content = blogPost.Content
            };

            return View(model);
        }

        // POST: Blog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var blogPost = _unitOfWork.BlogPost.Get(b => b.Id == id);
                if (blogPost == null)
                {
                    return NotFound();
                }

                blogPost.Title = model.Title;
                blogPost.Content = model.Content;

                // Handle image file upload
                if (model.photo != null && model.photo.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", model.photo.FileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.photo.CopyToAsync(fileStream);
                    }

                    blogPost.ImagePath = "/images/" + model.photo.FileName;
                }

                _unitOfWork.BlogPost.Update(blogPost);
                _unitOfWork.Save(); // Save changes
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Blog/Delete/5
        public IActionResult Delete(int id)
        {
            var blogPost = _unitOfWork.BlogPost.Get(b => b.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }

        // POST: Blog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var blogPost = _unitOfWork.BlogPost.Get(b => b.Id == id);
            if (blogPost == null)
            {
                return NotFound();
            }

            _unitOfWork.BlogPost.Remove(blogPost);
            _unitOfWork.Save(); // Save changes
            return RedirectToAction(nameof(Index));
        }
    }
}
