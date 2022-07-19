using hospins.Extensions;
using hospins.Infrastructure;
using hospins.Models;
using hospins.Repository.Data;
using hospins.Repository.Infrastructure;
using hospins.Repository.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace hospins.Areas.EmployeePanel.Controllers
{
    [UserAuthorize]
    public class LogbookController : EmployeePanelBaseControllerBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommonRepository<Category> _ICategoryRepository;
        private readonly ICommonRepository<SubCategory> _ISubCategoryRepository;
        private readonly ICommonRepository<Priority> _IPriorityRepository;
        private readonly ICommonRepository<Logbook> _ILogbookRepository;
        public LogbookController(IServiceProvider _service,
        ICommonRepository<Category> iCategoryRepository,
        ICommonRepository<SubCategory> iSubCategoryRepository,
        ICommonRepository<Priority> iPriorityRepository,
        ICommonRepository<Logbook> iLogbookRepository)
        {
            _serviceProvider = _service;
            _ICategoryRepository = iCategoryRepository;
            _ISubCategoryRepository = iSubCategoryRepository;
            _IPriorityRepository = iPriorityRepository;
            _ILogbookRepository = iLogbookRepository;
        }

        #region :: CATEGORY ::
        public IActionResult Category()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManageCategory(int id = 0)
        {
            var category = new CategoryModel();
            try
            {
                if (id != 0)
                {
                    category = _ICategoryRepository.GetById(id).ToModel();
                    
                }
                return PartialView(category);
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManageCategory");
                return PartialView(category);
            }
        }

        [HttpPost]
        public IActionResult ManageCategory(CategoryModel category)
        {
            try
            {
                var ObjCategory = category.CategoryId != 0 ? _ICategoryRepository.GetByIdWithNoTracking(category.CategoryId) : null;
                if (ObjCategory == null || !string.Equals(ObjCategory.Name.Trim().ToLower(), category.Name.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    var categoryDb = _ICategoryRepository.GetAll(x => string.Equals(x.Name.Trim().ToLower(), category.Name.Trim().ToLower()) && !x.IsDelete).FirstOrDefault();
                    if (categoryDb != null)
                        return Json(new { success = "false", ReturnMsg = "Category already exists.", PartialviewContent = this.RenderPartialViewToString("ManageCategory", category, _serviceProvider) });
                }
                if (ModelState.IsValid)
                {
                    if (category.CategoryId == 0)
                    {
                        Category CategoryObj = _ICategoryRepository.InsertAndGetObj(category.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "Category saved.", PartialviewContent = "" });
                    }
                    else
                    {
                        _ICategoryRepository.Update(category.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "Category updated.", PartialviewContent = "" });
                    }
                }
                else
                {
                    string _message = string.Join(Environment.NewLine, ModelState.Values
                                               .SelectMany(x => x.Errors)
                                               .Select(x => x.ErrorMessage));
                    return Json(new { success = "false", ReturnMsg = _message, PartialviewContent = this.RenderPartialViewToString("ManageCategory", category, _serviceProvider) });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManageCategory");
                return Json(new { success = "false", ReturnMsg = ex.Message, PartialviewContent = this.RenderPartialViewToString("ManageCategory", category, _serviceProvider) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StatusCategory(int Id)
        {
            try
            {
                var category = _ICategoryRepository.GetById(Id);
                if (category != null)
                {
                    category.IsActive = category.IsActive = !category.IsActive;
                    _ICategoryRepository.Update(category, CurrentContext.UserDetail.UserId, "status");
                    return Json(new { success = "true", ReturnMsg = "Category status change successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Category does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/StatusCategory");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMultipleCategory(string Id)
        {
            try
            {
                if (Id != null)
                {
                    //var ids = Id.Split(",").Select(t => Convert.ToInt32(t)).ToList();
                    //var category = _ICategoryRepository.GetAll().Where(t => ids.Contains(t.CategoryId));

                    SqlParameter[] param = {
                        new SqlParameter("@CategoryId", Id),
                        new SqlParameter("@UserId", CurrentContext.UserDetail.UserId)
                    };
                    var isSuccess = _ICategoryRepository.DeleteRecordSproc("DeleteCategory", param, ConfigurationSettings.DBConnection);

                    return Json(new { success = "true", ReturnMsg = "Category deleted successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Category does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/DeleteMultipleCategory");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }
        #endregion

        #region :: SUB CATEGORY ::
        public IActionResult SubCategory()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManageSubCategory(int id = 0)
        {
            var subCategory = new SubCategoryModel();
            try
            {
                if (id != 0)
                {
                    subCategory = _ISubCategoryRepository.GetById(id).ToModel();

                }
                return PartialView(subCategory);
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManageSubCategory");
                return PartialView(subCategory);
            }
        }

        [HttpPost]
        public IActionResult ManageSubCategory(SubCategoryModel subCategory)
        {
            try
            {
                var ObjSubCategory = subCategory.SubCategoryId != 0 ? _ISubCategoryRepository.GetByIdWithNoTracking(subCategory.SubCategoryId) : null;
                if (ObjSubCategory == null || !string.Equals(ObjSubCategory.Name.Trim().ToLower(), subCategory.Name.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    var subCategoryDb = _ISubCategoryRepository.GetAll(x => x.CategoryId == subCategory.CategoryId && string.Equals(x.Name.Trim().ToLower(), subCategory.Name.Trim().ToLower()) && !x.IsDelete).FirstOrDefault();
                    if (subCategoryDb != null)
                        return Json(new { success = "false", ReturnMsg = "SubCategory already exists.", PartialviewContent = this.RenderPartialViewToString("ManageSubCategory", subCategory, _serviceProvider) });
                }
                if (ModelState.IsValid)
                {
                    if (subCategory.SubCategoryId == 0)
                    {
                        SubCategory SubCategoryObj = _ISubCategoryRepository.InsertAndGetObj(subCategory.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "SubCategory saved.", PartialviewContent = "" });
                    }
                    else
                    {
                        _ISubCategoryRepository.Update(subCategory.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "SubCategory updated.", PartialviewContent = "" });
                    }
                }
                else
                {
                    string _message = string.Join(Environment.NewLine, ModelState.Values
                                               .SelectMany(x => x.Errors)
                                               .Select(x => x.ErrorMessage));
                    return Json(new { success = "false", ReturnMsg = _message, PartialviewContent = this.RenderPartialViewToString("ManageSubCategory", subCategory, _serviceProvider) });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManageSubCategory");
                return Json(new { success = "false", ReturnMsg = ex.Message, PartialviewContent = this.RenderPartialViewToString("ManageSubCategory", subCategory, _serviceProvider) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StatusSubCategory(int Id)
        {
            try
            {
                var subCategory = _ISubCategoryRepository.GetById(Id);
                if (subCategory != null)
                {
                    subCategory.IsActive = subCategory.IsActive = !subCategory.IsActive;
                    _ISubCategoryRepository.Update(subCategory, CurrentContext.UserDetail.UserId, "status");
                    return Json(new { success = "true", ReturnMsg = "SubCategory status change successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "SubCategory does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/StatusSubCategory");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMultipleSubCategory(string Id)
        {
            try
            {
                if (Id != null)
                {
                    SqlParameter[] param = {
                        new SqlParameter("@SubCategoryId", Id),
                        new SqlParameter("@UserId", CurrentContext.UserDetail.UserId)
                    };
                    var isSuccess = _ISubCategoryRepository.DeleteRecordSproc("DeleteSubCategory", param, ConfigurationSettings.DBConnection);

                    return Json(new { success = "true", ReturnMsg = "SubCategory deleted successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "SubCategory does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/DeleteMultipleSubCategory");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }
        #endregion

        #region :: PRIORITY ::
        public IActionResult Priority()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManagePriority(int id = 0)
        {
            var category = new PriorityModel();
            try
            {
                if (id != 0)
                {
                    category = _IPriorityRepository.GetById(id).ToModel();

                }
                return PartialView(category);
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManagePriority");
                return PartialView(category);
            }
        }

        [HttpPost]
        public IActionResult ManagePriority(PriorityModel category)
        {
            try
            {
                var ObjPriority = category.PriorityId != 0 ? _IPriorityRepository.GetByIdWithNoTracking(category.PriorityId) : null;
                if (ObjPriority == null || !string.Equals(ObjPriority.Name.Trim().ToLower(), category.Name.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    var categoryDb = _IPriorityRepository.GetAll(x => string.Equals(x.Name.Trim().ToLower(), category.Name.Trim().ToLower()) && !x.IsDelete).FirstOrDefault();
                    if (categoryDb != null)
                        return Json(new { success = "false", ReturnMsg = "Priority already exists.", PartialviewContent = this.RenderPartialViewToString("ManagePriority", category, _serviceProvider) });
                }
                if (ModelState.IsValid)
                {
                    if (category.PriorityId == 0)
                    {
                        Priority PriorityObj = _IPriorityRepository.InsertAndGetObj(category.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "Priority saved.", PartialviewContent = "" });
                    }
                    else
                    {
                        _IPriorityRepository.Update(category.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "Priority updated.", PartialviewContent = "" });
                    }
                }
                else
                {
                    string _message = string.Join(Environment.NewLine, ModelState.Values
                                               .SelectMany(x => x.Errors)
                                               .Select(x => x.ErrorMessage));
                    return Json(new { success = "false", ReturnMsg = _message, PartialviewContent = this.RenderPartialViewToString("ManagePriority", category, _serviceProvider) });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManagePriority");
                return Json(new { success = "false", ReturnMsg = ex.Message, PartialviewContent = this.RenderPartialViewToString("ManagePriority", category, _serviceProvider) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StatusPriority(int Id)
        {
            try
            {
                var category = _IPriorityRepository.GetById(Id);
                if (category != null)
                {
                    category.IsActive = category.IsActive = !category.IsActive;
                    _IPriorityRepository.Update(category, CurrentContext.UserDetail.UserId, "status");
                    return Json(new { success = "true", ReturnMsg = "Priority status change successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Priority does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/StatusPriority");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMultiplePriority(string Id)
        {
            try
            {
                if (Id != null)
                {
                    //var ids = Id.Split(",").Select(t => Convert.ToInt32(t)).ToList();
                    //var category = _IPriorityRepository.GetAll().Where(t => ids.Contains(t.PriorityId));

                    SqlParameter[] param = {
                        new SqlParameter("@PriorityId", Id),
                        new SqlParameter("@UserId", CurrentContext.UserDetail.UserId)
                    };
                    var isSuccess = _IPriorityRepository.DeleteRecordSproc("DeletePriority", param, ConfigurationSettings.DBConnection);

                    return Json(new { success = "true", ReturnMsg = "Priority deleted successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Priority does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/DeleteMultiplePriority");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }
        #endregion

        #region :: LOGBOOK ::
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManageLogbook(int id = 0)
        {
            var logbook = new LogbookModel();
            try
            {
                if (id != 0)
                {
                    logbook = _ILogbookRepository.GetById(id).ToModel();
                    logbook.SequnceId = logbook.LogbookId;
                }
                else
                {
                    logbook.SequnceId = _ILogbookRepository.GetAll().Max(t => t.LogbookId) + 1;
                }
                return View(logbook);
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/Index");
                return View(logbook);
            }
        }

        [HttpPost]
        public IActionResult ManageLogbook(LogbookModel logbook)
        {
            string success = string.Empty;
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    var ObjLogbook = logbook.LogbookId != 0 ? _ILogbookRepository.GetByIdWithNoTracking(logbook.LogbookId) : null;
                    if (ObjLogbook == null || !string.Equals(ObjLogbook.Name.Trim().ToLower(), logbook.Name.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))
                    {
                        var sercentre = _ILogbookRepository.GetAll(x => string.Equals(x.Name.Trim().ToLower(), logbook.Name.Trim().ToLower()) && !x.IsDelete).FirstOrDefault();
                        if (sercentre != null)
                        {
                            ViewBag.success = "false";
                            ViewBag.message = "Logbook name already exist.";
                            return View(logbook);
                        }
                    }
                    if (logbook.LogbookId == 0)
                    {
                        var _logbook = logbook.ToEntity();
                        _logbook.CreatedDate = DateTime.Now;
                        _logbook.CreatedBy = CurrentContext.UserDetail.UserId;
                        var logbookObj = _ILogbookRepository.InsertAndGetObj(_logbook, CurrentContext.UserDetail.UserId);

                        success = "true";
                        message = "Logbook saved.";
                    }
                    else
                    {
                        var _logbook = logbook.ToEntity();
                        _logbook.CreatedDate = ObjLogbook.CreatedDate;
                        _logbook.CreatedBy = ObjLogbook.CreatedBy;
                        _logbook.UpdatedDate = DateTime.Now;
                        _logbook.UpdatedBy = CurrentContext.UserDetail.UserId;
                        _ILogbookRepository.Update(_logbook, CurrentContext.UserDetail.UserId);
                        success = "true";
                        message = "Logbook updated.";
                    }
                }
                else
                {
                    string _message = string.Join(Environment.NewLine, ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage));
                    success = "false";
                    message = _message;
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManageLogbook");
                success = "false";
                message = ex.Message;
            }

            ViewBag.success = success;
            ViewBag.message = message;

            TempData["LogbookSuccess"] = success;
            TempData["LogbookMessage"] = message;

            if (success == "true")
            {
                return RedirectToAction("Index", "Logbook");
            }
            else
            {
                return View(logbook);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMultipleLogbook(string Id)
        {
            try
            {
                if (Id != null)
                {
                    SqlParameter[] param = {
                        new SqlParameter("@LogbookId", Id),
                        new SqlParameter("@UserId", CurrentContext.UserDetail.UserId)
                    };
                    var isSuccess = _ILogbookRepository.DeleteRecordSproc("DeleteLogbook", param, ConfigurationSettings.DBConnection);

                    return Json(new { success = "true", ReturnMsg = "Logbook deleted successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Logbook does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/DeleteMultipleLogbook");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }

        [HttpGet]
        public IActionResult BindSubCategory(int id)
        {
            try
            {
                var subCategories = _ISubCategoryRepository.GetAll(t => t.CategoryId == id && t.IsDelete == false && t.IsActive == true);
                return Json(new { success = "true", ReturnMsg = subCategories });
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/BindModels");
                return Json(new { success = "false", ReturnMsg = ex.Message });
            }
        }
        #endregion
    }
}