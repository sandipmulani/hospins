using AutoMapper;
using hospins.Extensions;
using hospins.Infrastructure;
using hospins.Models;
using hospins.Repository.Data;
using hospins.Repository.Infrastructure;
using hospins.Repository.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace hospins.Controllers
{
    [UserAuthorize]
    public class EmployeeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommonRepository<Designation> _IDesignationRepository;
        private readonly ICommonRepository<Employee> _IEmployeeRepository;
        private readonly ICommonRepository<EmployeeAddress> _IEmployeeAddressRepository;
        private readonly ICommonRepository<EmployeeSalarySetup> _IEmployeeSalarySetupRepository;
        private readonly ICommonRepository<EmployeeHistory> _IEmployeeHistoryRepository;
        private readonly ICommonRepository<EmployeeDocument> _IEmployeeDocumentRepository;
        public EmployeeController(IMapper mapper,IServiceProvider _service,
        ICommonRepository<Designation> iDesignationRepository,
        ICommonRepository<Employee> iEmployeeRepository,
        ICommonRepository<EmployeeAddress> iEmployeeAddressRepository,
        ICommonRepository<EmployeeSalarySetup> iEmployeeSalarySetupRepository,
        ICommonRepository<EmployeeHistory> iEmployeeHistoryRepository,
        ICommonRepository<EmployeeDocument> iEmployeeDocumentRepository)
        {
            _mapper = mapper;
            _serviceProvider = _service;
            _IDesignationRepository = iDesignationRepository;
            _IEmployeeRepository = iEmployeeRepository;
            _IEmployeeAddressRepository = iEmployeeAddressRepository;
            _IEmployeeSalarySetupRepository = iEmployeeSalarySetupRepository;
            _IEmployeeHistoryRepository = iEmployeeHistoryRepository;
            _IEmployeeDocumentRepository = iEmployeeDocumentRepository;
        }

        #region :: Designation ::
        public IActionResult Designation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManageDesignation(int id = 0)
        {
            var Designation = new DesignationModel();
            try
            {
                if (id != 0)
                {
                    Designation = _IDesignationRepository.GetById(id).ToModel();

                }
                return PartialView(Designation);
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManageDesignation");
                return PartialView(Designation);
            }
        }

        [HttpPost]
        public IActionResult ManageDesignation(DesignationModel designation)
        {
            try
            {
                var ObjDesignation = designation.DesignationId != 0 ? _IDesignationRepository.GetByIdWithNoTracking(designation.DesignationId) : null;
                if (ObjDesignation == null || !string.Equals(ObjDesignation.Name.Trim().ToLower(), designation.Name.Trim().ToLower(), StringComparison.OrdinalIgnoreCase))
                {
                    var DesignationDb = _IDesignationRepository.GetAll(x => string.Equals(x.Name.Trim().ToLower(), designation.Name.Trim().ToLower()) && !x.IsDelete).FirstOrDefault();
                    if (DesignationDb != null)
                        return Json(new { success = "false", ReturnMsg = "Designation already exists.", PartialviewContent = this.RenderPartialViewToString("ManageDesignation", designation, _serviceProvider) });
                }
                if (ModelState.IsValid)
                {
                    if (designation.DesignationId == 0)
                    {
                        Designation DesignationObj = _IDesignationRepository.InsertAndGetObj(designation.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "Designation saved.", PartialviewContent = "" });
                    }
                    else
                    {
                        _IDesignationRepository.Update(designation.ToEntity(), CurrentContext.UserDetail.UserId);
                        return Json(new { success = "true", ReturnMsg = "Designation updated.", PartialviewContent = "" });
                    }
                }
                else
                {
                    string _message = string.Join(Environment.NewLine, ModelState.Values
                                               .SelectMany(x => x.Errors)
                                               .Select(x => x.ErrorMessage));
                    return Json(new { success = "false", ReturnMsg = _message, PartialviewContent = this.RenderPartialViewToString("ManageDesignation", designation, _serviceProvider) });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Logbook/ManageDesignation");
                return Json(new { success = "false", ReturnMsg = ex.Message, PartialviewContent = this.RenderPartialViewToString("ManageDesignation", designation, _serviceProvider) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StatusDesignation(int Id)
        {
            try
            {
                var model = _IDesignationRepository.GetById(Id);
                if (model != null)
                {
                    model.IsActive = model.IsActive = !model.IsActive;
                    _IDesignationRepository.Update(model, CurrentContext.UserDetail.UserId, "status");
                    return Json(new { success = "true", ReturnMsg = "Designation status change successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Designation does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Employee/StatusDesignation");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMultipleDesignation(string Id)
        {
            try
            {
                if (Id != null)
                {
                    SqlParameter[] param = {
                        new SqlParameter("@DesignationId", Id),
                        new SqlParameter("@UserId", CurrentContext.UserDetail.UserId)
                    };
                    var isSuccess = _IDesignationRepository.DeleteRecordSproc("DeleteDesignation", param, ConfigurationSettings.DBConnection);

                    return Json(new { success = "true", ReturnMsg = "Designation deleted successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Designation does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Employee/DeleteMultipleDesignation");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }
        #endregion

        #region :: Employee ::
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ManageEmployee(int id = 0)
        {
            var employee = new EmployeeModel();
            try
            {
                #region :: EMPLOYEE ::
                employee = _IEmployeeRepository.GetById(id).ToModel() ?? new EmployeeModel();
                if (employee != null)
                {
                    employee.CurrentAddress = _IEmployeeAddressRepository.GetAll().FirstOrDefault(t => t.EmployeeId == employee.EmployeeId && t.IsDelete == false && t.IsActive == true && t.AddressTypeId == 1)?.ToModel();
                    employee.PermanentAddress = _IEmployeeAddressRepository.GetAll().FirstOrDefault(t => t.EmployeeId == employee.EmployeeId && t.IsDelete == false && t.IsActive == true && t.AddressTypeId == 2)?.ToModel();
                    employee.EmployeeSalarySetup = _IEmployeeSalarySetupRepository.GetAll().FirstOrDefault(t => t.EmployeeId == employee.EmployeeId && t.IsDelete == false && t.IsActive == true)?.ToModel();
                    employee.EmployeeHistory = _IEmployeeHistoryRepository.GetAll(x => x.EmployeeId == employee.EmployeeId && x.IsDelete == false)?.ToModel();
                    employee.EmployeeDocument = _IEmployeeDocumentRepository.GetAll(x => x.EmployeeId == employee.EmployeeId && x.IsDelete == false)?.ToModel();
                }
                #endregion

                if ((employee?.EmployeeHistory?.Count() ?? 0) == 0)
                {
                    employee.EmployeeHistory.AddRange(Enumerable.Repeat(new EmployeeHistoryModel(), 1));
                }
                if ((employee?.EmployeeDocument?.Count() ?? 0) == 0)
                {
                    employee.EmployeeDocument.AddRange(Enumerable.Repeat(new EmployeeDocumentModel(), 1));
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                ex.SetLog("Employee/ManageEmployee");
                return View(employee);
            }
        }

        [HttpPost]
        public IActionResult ManageEmployee(EmployeeModel employee)
        {
            string success = string.Empty;
            string message = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    #region :: EMPLOYEE ::
                    if (employee.EmployeeId == 0)
                    {
                        var _employee = employee.ToEntity();
                        _employee.CreatedBy = CurrentContext.UserDetail.UserId;
                        _employee.CreatedDate = DateTime.Now;
                        _employee.IsActive = true;
                        _employee.IsDelete = false;
                        _employee = _IEmployeeRepository.InsertAndGetObj(_employee, CurrentContext.UserDetail.UserId);
                        employee.EmployeeId = _employee.EmployeeId;

                        success = "true";
                        message = "Employee saved.";
                    }
                    else
                    {
                        var _employee = employee.ToEntity();
                        _employee.CreatedBy = CurrentContext.UserDetail.UserId;
                        _employee.CreatedDate = DateTime.Now;
                        _employee.ModifyDate = DateTime.Now;
                        _employee.ModifyBy = CurrentContext.UserDetail.UserId;
                        _employee.IsActive = true;
                        _employee.IsDelete = false;
                        _IEmployeeRepository.Update(_employee, CurrentContext.UserDetail.UserId);

                        success = "true";
                        message = "Employee updated.";
                    }
                    #endregion

                    #region :: Employee Current Address ::
                    var currentAddress = _IEmployeeAddressRepository.GetAll().FirstOrDefault(t => t.EmployeeId == employee.EmployeeId && t.IsDelete == false && t.IsActive == true && t.AddressTypeId == 1);
                    if (currentAddress != null && employee.CurrentAddress != null)
                    {
                        var _currentAddress = _mapper.Map(employee.CurrentAddress, currentAddress);
                        _IEmployeeAddressRepository.Update(_currentAddress, CurrentContext.UserDetail.UserId);
                    }
                    else if (employee.CurrentAddress != null)
                    {
                        var _currentAddress = _mapper.Map<EmployeeAddress>(employee.CurrentAddress);
                        _currentAddress.EmployeeId = employee.EmployeeId;
                        _currentAddress.AddressTypeId = 1;
                        _currentAddress = _IEmployeeAddressRepository.InsertAndGetObj(_currentAddress, CurrentContext.UserDetail.UserId);
                    }
                    else if (currentAddress != null)
                    {
                        currentAddress.IsDelete = true;
                        _IEmployeeAddressRepository.Update(currentAddress, CurrentContext.UserDetail.UserId);
                    }
                    #endregion

                    #region :: Employee Permanent Address ::
                    var permanentAddress = _IEmployeeAddressRepository.GetAll().FirstOrDefault(t => t.EmployeeId == employee.EmployeeId && t.IsDelete == false && t.IsActive == true && t.AddressTypeId == 2);
                    if (permanentAddress != null && employee.PermanentAddress != null)
                    {
                        var _permanentAddress = _mapper.Map(employee.PermanentAddress, permanentAddress);
                        _IEmployeeAddressRepository.Update(_permanentAddress, CurrentContext.UserDetail.UserId);
                    }
                    else if (employee.PermanentAddress != null)
                    {
                        var _permanentAddress = _mapper.Map<EmployeeAddress>(employee.PermanentAddress);
                        _permanentAddress.EmployeeId = employee.EmployeeId;
                        _permanentAddress.AddressTypeId = 2;
                        _permanentAddress = _IEmployeeAddressRepository.InsertAndGetObj(_permanentAddress, CurrentContext.UserDetail.UserId);
                    }
                    else if (permanentAddress != null)
                    {
                        permanentAddress.IsDelete = true;
                        _IEmployeeAddressRepository.Update(permanentAddress, CurrentContext.UserDetail.UserId);
                    }
                    #endregion

                    #region :: Employee Salary Setup ::
                    var employeeSalarySetup = _IEmployeeSalarySetupRepository.GetAll().FirstOrDefault(t => t.EmployeeId == employee.EmployeeId && t.IsDelete == false && t.IsActive == true);
                    if (employeeSalarySetup != null && employee.EmployeeSalarySetup != null)
                    {
                        var _employeeSalarySetup = _mapper.Map(employee.EmployeeSalarySetup, employeeSalarySetup);
                        _IEmployeeSalarySetupRepository.Update(_employeeSalarySetup, CurrentContext.UserDetail.UserId);
                    }
                    else if (employee.EmployeeSalarySetup != null)
                    {
                        var _employeeSalarySetup = _mapper.Map<EmployeeSalarySetup>(employee.EmployeeSalarySetup);
                        _employeeSalarySetup.EmployeeId = employee.EmployeeId;
                        _employeeSalarySetup = _IEmployeeSalarySetupRepository.InsertAndGetObj(_employeeSalarySetup, CurrentContext.UserDetail.UserId);
                    }
                    else if (employeeSalarySetup != null)
                    {
                        employeeSalarySetup.IsDelete = true;
                        _IEmployeeSalarySetupRepository.Update(employeeSalarySetup, CurrentContext.UserDetail.UserId);
                    }
                    #endregion

                    #region :: Employee History ::
                    var employeeHistory = _IEmployeeHistoryRepository.GetAll(x => x.EmployeeId == employee.EmployeeId && x.IsDelete == false);
                    _IEmployeeHistoryRepository.DeleteRange(employeeHistory, CurrentContext.UserDetail.UserId);
                    if (employee.EmployeeHistory?.Count > 0)
                    {
                        employee.EmployeeHistory = employee.EmployeeHistory.Where(x => !string.IsNullOrEmpty(x.CompanyName)).ToList();
                        var _employeeHistory = employee.EmployeeHistory.ToEntity();
                        _employeeHistory.ForEach(x => { x.EmployeeId = employee.EmployeeId; });
                        _IEmployeeHistoryRepository.InsertRange(_employeeHistory, CurrentContext.UserDetail.UserId);
                    }
                    #endregion

                    #region :: Employee Document ::
                    var employeeDocument = _IEmployeeDocumentRepository.GetAll(x => x.EmployeeId == employee.EmployeeId && x.IsDelete == false);
                    _IEmployeeDocumentRepository.DeleteRange(employeeDocument, CurrentContext.UserDetail.UserId);
                    if (employee.EmployeeDocument?.Count > 0)
                    {
                        employee.EmployeeDocument = employee.EmployeeDocument.Where(x => (!string.IsNullOrEmpty(x.FileName) || x.File != null) && x.DocumentTypeId != 0).ToList();
                        var _employeeDocument = employee.EmployeeDocument.ToEntity();
                        _employeeDocument.ForEach(x => { x.EmployeeId = employee.EmployeeId; });
                        _IEmployeeDocumentRepository.InsertRange(_employeeDocument, CurrentContext.UserDetail.UserId);
                    }
                    #endregion
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
                ex.SetLog("Employee/ManageEmployee");
                success = "false";
                message = ex.Message;
            }

            ViewBag.success = success;
            ViewBag.message = message;

            TempData["EmployeeSuccess"] = success;
            TempData["EmployeeMessage"] = message;

            if (success == "true")
            {
                return RedirectToAction("Index", "Employee");
            }
            else
            {
                return View(employee);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMultipleEmployee(string Id)
        {
            try
            {
                if (Id != null)
                {
                    SqlParameter[] param = {
                        new SqlParameter("@EmployeeId", Id),
                        new SqlParameter("@UserId", CurrentContext.UserDetail.UserId)
                    };
                    var isSuccess = _IEmployeeRepository.DeleteRecordSproc("DeleteEmployee", param, ConfigurationSettings.DBConnection);

                    return Json(new { success = "true", ReturnMsg = "Employee deleted successfully.", PartialviewContent = "" });
                }
                else
                {
                    return Json(new { success = "false", ReturnMsg = "Employee does not exist.", PartialviewContent = "" });
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Master/DeleteMultipleEmployee");
                return Json(new { success = "false", ReturnMsg = "Error", PartialviewContent = "" });
            }
        }
        #endregion

        #region :: Document Type ::
        public IActionResult DocumentType()
        {
            return View();
        }
        #endregion
    }
}