using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hospins.Areas.EmployeePanel.Controllers
{
    public class HomeController : EmployeePanelBaseControllerBase
    {
        public async Task<IActionResult> Index()
        {
            return View();
        }
    }
}