using AK.DataAccess.Data;
using AK.DataAccess.Repository.IRepository;
using AK.Models;
using AK.Models.ViewModels;
using AK.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AKEcom.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles =SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitofwork;

        public CompanyController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitofwork = unitOfWork;
        }

        public IActionResult Index()
        {

            List<Company> objCompanyList = _unitofwork.Company.GetAll().ToList();
           
            return View(objCompanyList);
        }

        public IActionResult Upsert(int? id)
        {


            if (id == null || id ==0) { 

                //create
                return View(new Company());
            }
            else
            {
                //update
                Company companyObj = _unitofwork.Company.Get(u => u.Id == id);
                return View (companyObj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company companyObj)
        {
            if (ModelState.IsValid)
            {
               
                if (companyObj.Id == 0)
                {
                    _unitofwork.Company.Add(companyObj);
                }
                else
                {
                    _unitofwork.Company.Update(companyObj);

                }

                _unitofwork.Save();
                TempData["success"] = "Company Created Successfully";
                return RedirectToAction("Index");
            }
            else
            {

             
                return View(companyObj);
            }
           

        }

       


       
        #region APICALL
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitofwork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }



        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companytoBeDeleted = _unitofwork.Company.Get(u => u.Id == id);
            if (companytoBeDeleted == null )
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }

           

            _unitofwork.Company.Remove(companytoBeDeleted);
            _unitofwork.Save(); 
            return Json( new {success=true, message = "Delete Successfull"});
        }


        #endregion



    }
}
