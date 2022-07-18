using hospins.Infrastructure;
using hospins.Repository.Infrastructure;
using hospins.Repository.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using hospins.Repository.Data;

namespace hospins.Areas.EmployeePanel.Controllers
{
    [UserAuthorize]
    public class DashboardController : EmployeePanelBaseControllerBase
    {
        private readonly ICommonRepository<State> _IStateRepository;
        public DashboardController(ICommonRepository<State> iStateRepository)
        {
            _IStateRepository = iStateRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Logout()
        {
            if (CurrentContext.UserDetail != null)
            {
                CurrentContext.UserDetail = null;
            }
            return RedirectToAction("Index", "Home");
        }





        #region Common Grid Methods
        /// <summary>
        /// Common Grid Function : Bind list using SP 
        /// </summary>
        /// <returns>Return list</returns>
        [HttpPost]
        public string GetGridData(List<SearchGrid> SearchParams)
        {
            try
            {
                string mode = Convert.ToString(Request.Form["mode"]);
                string where = "";
                if (!string.IsNullOrEmpty(Convert.ToString(Request.Form["SearchParams"])))
                {
                    where = Convert.ToString(Request.Form["SearchParams"]);
                }
                where += Common.GetFilterClause(SearchParams);
                ColumnConfig columnConfig = new ColumnConfig(mode, HttpContext, where);
                return columnConfig.gridParams.GetData();
            }
            catch (Exception ex)
            {
                ex.SetLog("Dashboard/GetGridData");
                throw;
            }
        }

        /// <summary>
        ///  Export to Excel
        /// </summary>
        /// <returns>return excel file</returns>
        [HttpPost]
        public IActionResult ExportData()
        {
            try
            {
                var SearchParams = new List<SearchGrid>();
                var output = JsonConvert.DeserializeObject<dynamic>(Request.Form["filters"]);
                if (output.Count > 0)
                    SearchParams = JsonConvert.DeserializeObject<List<SearchGrid>>(Request.Form["filters"]);

                var where = Common.GetFilterClause(SearchParams);
                string mode = Convert.ToString(Request.Form["mode"]);
                ColumnConfig columnConfig = new ColumnConfig(mode, HttpContext, where);
                if (string.IsNullOrEmpty(columnConfig.gridParams.WhereClause)) columnConfig.gridParams.WhereClause = "1=1";
                columnConfig.gridParams.ExportData();
                return View();
            }
            catch (Exception ex)
            {
                ex.SetLog("Dashboard/ExportData");
                throw;
            }
        }
        #endregion

        [HttpGet]
        public IActionResult BindState(int id)
        {
            try
            {
                var states = _IStateRepository.GetAll(t => t.CountryId == id);
                return Json(new { success = "true", ReturnMsg = states });
            }
            catch (Exception ex)
            {
                ex.SetLog("Dashboard/BindState");
                return Json(new { success = "false", ReturnMsg = ex.Message });
            }
        }
    }
}
