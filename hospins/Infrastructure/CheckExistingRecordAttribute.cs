using System;
using System.Collections.Generic;
using hospins.Infrastructure;
using hospins.Repository.Infrastructure;
using hospins.Repository.ServiceContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace hospins.Infrastructure
{
    public class CheckExistingRecordAttribute : ActionFilterAttribute
    {
        public string TableName { get; set; }
        public string IdParamName { get; set; }
        public string ExcludeTables { get; set; } = "";

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var isConfirm = filterContext.HttpContext.Request.Headers["__isConfirm"];
            if(isConfirm != "true")
            {
                string deleteIds = "";
                if (filterContext.ActionArguments.ContainsKey(IdParamName))
                {
                    deleteIds = Convert.ToString(filterContext.ActionArguments[IdParamName] ?? "");
                }
                if (!string.IsNullOrEmpty(deleteIds) && deleteIds != "0")
            {
                var _IRepository = new HttpContextAccessor().HttpContext.RequestServices.GetService(typeof(ICheckExistingRepository)) as ICheckExistingRepository;
                var _paraDic = new Dictionary<string, object>
                    {
                        { "table_name", TableName },
                        { "id", deleteIds },
                        { "exclude_tables", ExcludeTables }
                    };
                var result = _IRepository.CheckChildRecordExistsForDelete("CheckChildRecordExists", _paraDic, ConfigurationSettings.DBConnection);
                if (result == "1")
                {
                    //filterContext.Result = new JsonResult(new { success = "false", ReturnMsg = "You can't delete this record, this record is being used by other records too." });
                    filterContext.Result = new JsonResult(new { success = "false", ReturnMsg = "childExists" });
                }
            }
            }
            base.OnActionExecuting(filterContext);
        }
    }

    public class ForDelete { }
}
