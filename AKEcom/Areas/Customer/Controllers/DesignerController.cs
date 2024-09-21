using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Models.ViewModels;
using AK.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AKEcom.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class DesignerController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public DesignerController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        // 1. Display a list of designers
        [Authorize(Roles = "Customer, InteriorDesigner")]
        public IActionResult Designer()
        {
            var designers = _unitOfWork.ApplicationUser.GetAll()
                .Where(u => _userManager.IsInRoleAsync(u, SD.Role_InteriorDesigner).Result)
                .ToList();

            if (!designers.Any())
            {
                ViewBag.Message = "No designers found.";
            }

            return View(designers);
        }

        // 2. View portfolio for a specific designer
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Portfolio(string designerId)
        {
            var designer = await _unitOfWork.ApplicationUser.GetFirstOrDefaultAsync(u => u.Id == designerId);
            if (designer == null)
            {
                return NotFound("Designer not found.");
            }

            ViewData["DesignerName"] = designer.Name;

            var portfolios = _unitOfWork.Portfolio.GetAll(p => p.UserId == designerId).ToList();
            if (!portfolios.Any())
            {
                return NotFound("No portfolios found for this designer.");
            }

            return View(portfolios);
        }

        // 3. Show consultation request form
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult RequestConsultation(string designerId)
        {
            var consultationViewModel = new ConsultationViewModel
            {
                DesignerId = designerId
            };

            return View(consultationViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> SubmitConsultation(ConsultationViewModel model)
        {
            var loggedInEmail = User.FindFirstValue(ClaimTypes.Email);

            if (model.Email != loggedInEmail)
            {
                ModelState.AddModelError("Email", "Please use your logged-in email address.");
            }

            if (ModelState.IsValid)
            {
                var consultationRequest = new ConsultationRequest
                {
                    ApplicationUserId = model.DesignerId,
                    Name = model.Name,
                    Email = model.Email,
                    Message = model.Message,
                    DateRequested = DateTime.Now,
                    IsApproved = false // Default status
                };

                _unitOfWork.ConsultationRequest.Add(consultationRequest);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Consultation request submitted successfully!";
                return RedirectToAction("Designer");
            }

            return View("RequestConsultation", model);
        }


        [Authorize(Roles = "Customer, InteriorDesigner")]
        public async Task<IActionResult> DesignerRequests()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            IEnumerable<ConsultationRequest> consultationRequests;

            if (User.IsInRole(SD.Role_InteriorDesigner))
            {
                // Designer sees all requests assigned to them
                consultationRequests = await _unitOfWork.ConsultationRequest.GetAllAsync(c => c.ApplicationUserId == userId);
            }
            else
            {
                // Customer sees only their requests based on their email
                var email = User.FindFirstValue(ClaimTypes.Email);
                consultationRequests = await _unitOfWork.ConsultationRequest.GetAllAsync(c => c.Email == email);
            }

            if (!consultationRequests.Any())
            {
                ViewBag.Message = "No consultation requests found.";
                Console.WriteLine($"User {User.Identity.Name} has no consultation requests.");
            }
            else
            {
                Console.WriteLine($"User {User.Identity.Name} has {consultationRequests.Count()} consultation requests.");
            }

            return View(consultationRequests);
        }



        // 6. Approve a consultation request
        [HttpPost]
        public async Task<IActionResult> ApproveConsultation(int requestId)
        {
            var request = await _unitOfWork.ConsultationRequest.GetFirstOrDefaultAsync(r => r.Id == requestId);
            if (request != null)
            {
                request.IsApproved = true;
                _unitOfWork.ConsultationRequest.Update(request);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Consultation approved.";
            }

            return RedirectToAction(nameof(DesignerRequests), new { designerId = request.ApplicationUserId });
        }

        // 7. Reject a consultation request
        [HttpPost]
        public async Task<IActionResult> RejectConsultation(int requestId)
        {
            var request = await _unitOfWork.ConsultationRequest.GetFirstOrDefaultAsync(r => r.Id == requestId);
            if (request != null)
            {
                _unitOfWork.ConsultationRequest.Remove(request);
                await _unitOfWork.SaveAsync();

                TempData["Success"] = "Consultation request rejected.";
            }

            return RedirectToAction(nameof(DesignerRequests), new { designerId = request.ApplicationUserId });
        }
    }
}
