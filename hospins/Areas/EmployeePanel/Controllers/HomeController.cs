using System;
using System.IO;
using System.Web;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hospins.Models;
using hospins.UserAgents;
using hospins.Repository.ServiceContract;
using hospins.Infrastructure;
using hospins.Repository.Infrastructure;
using hospins.Repository.Data;

namespace hospins.Areas.EmployeePanel.Controllers
{
    public class HomeController : EmployeePanelBaseControllerBase
    {
        private readonly IUserRepository _IUserInfoRepository;
        private readonly ICommonRepository<User> _IUserRepository;
        public HomeController(IUserRepository iuserinforepository, 
        ICommonRepository<User> iUserRepository)
        {
            _IUserInfoRepository = iuserinforepository;
            _IUserRepository = iUserRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var objUser = _IUserInfoRepository.CheckLogin(model.UserName, model.Password,(int)EnmUserType.Employee);
                    if (objUser != null)
                    {
                        CurrentContext.UserDetail = objUser;
                        return RedirectToAction("Index", "Dashboard", new { area = "EmployeePanel" });
                    }
                    else
                    {
                        ViewBag.success = "false";
                        ViewBag.Errmessage = "Invalid username or password.";
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.success = "false";
                    ViewBag.Errmessage = "Invalid username or password.";
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ViewBag.success = "false";
                ViewBag.Errmessage = "some error.";
                ex.SetLog("test");
                return View(model);
            }
        }
    }
}
