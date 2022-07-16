using Microsoft.AspNetCore.Http;
using hospins.Repository.Infrastructure;

namespace hospins.Infrastructure
{
    /// <summary>
    /// for configure all grids of system.
    /// </summary>
    public class ColumnConfig
    {
        public GridParams gridParams = new GridParams();

        /// <summary>
        /// init grid configuration
        /// </summary>
        /// <param name="mode">keyword for identify grid</param>
        public ColumnConfig(string mode, HttpContext _HTTPContext, string Where = "")
        {
            gridParams._HTTPContext = _HTTPContext;
            gridParams.PageNumber = 1;
            gridParams.RecordPerPage = 10;
            switch (mode)
            {
                #region :: Users/Role ::
                case "User":
                    {
                        gridParams.ColumnsName = "U.UserId,U.RoleId,R.RoleName,U.Username,(U.FirstName +' '+ U.LastName) as Name,U.Email as Email,U.Mobile as Mobile,U.IsActive,U.IsDelete";
                        gridParams.SortColumn = "UserId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "[User] U WITH(NOLOCK) INNER JOIN Role R WITH(NOLOCK) ON R.RoleId=U.RoleId";
                        gridParams.WhereClause = " IsDelete=0 " + Where;
                        gridParams.ExportedFileName = "UserList";
                        break;
                    }
                case "Role":
                    {
                        gridParams.ColumnsName = "RoleId,RoleName,IsActive";
                        gridParams.SortColumn = "RoleId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "Role";
                        gridParams.WhereClause = " IsActive=1  ";
                        gridParams.ExportedFileName = "RoleList";
                        break;
                    }
                #endregion
                case "Category":
                    {
                        gridParams.ColumnsName = "CategoryId,Name,SortOrder,IsActive,IsDelete";
                        gridParams.SortColumn = "CategoryId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "Category";
                        gridParams.WhereClause = " IsDelete=0  " + Where;
                        gridParams.ExportedFileName = "Category List";
                        break;
                    }
                case "SubCategory":
                    {
                        gridParams.ColumnsName = "SC.SubCategoryId,C.Name as Category,SC.Name,SC.SortOrder,SC.IsActive,SC.IsDelete";
                        gridParams.SortColumn = "SubCategoryId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "SubCategory SC LEFT JOIN Category C ON C.CategoryId = SC.CategoryId";
                        gridParams.WhereClause = " IsDelete=0  " + Where;
                        gridParams.ExportedFileName = "SubCategory List";
                        break;
                    }
                case "Priority":
                    {
                        gridParams.ColumnsName = "PriorityId,Name,SortOrder,IsActive,IsDelete";
                        gridParams.SortColumn = "PriorityId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "Priority";
                        gridParams.WhereClause = " IsDelete=0  " + Where;
                        gridParams.ExportedFileName = "Priority List";
                        break;
                    }
                case "Logbook":
                    {
                        gridParams.ColumnsName = "LB.LogbookId,LB.Name,FORMAT(LB.Date, 'MMM, dd yyyy') as Date,C.Name Category,SC.Name as SubCategory,LB.Description,U.FirstName+' '+U.LastName as AssisgnTo,P.Name as PriorityId,LB.IsActive,LB.IsDelete,FORMAT(LB.CreatedDate, 'MMM, dd yyyy') as CreatedDate,CU.FirstName+' '+CU.LastName as CreatedBy";
                        gridParams.SortColumn = "LogbookId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "[dbo].[Logbook] LB LEFT JOIN [dbo].[Category] C on C.CategoryId = LB.CategoryId LEFT JOIN [dbo].[SubCategory] SC on SC.SubCategoryId = LB.SubCategoryId LEFT JOIN [dbo].[User] U on U.UserId = LB.AssisgnTo LEFT JOIN [dbo].[Priority] P on P.PriorityId = LB.PriorityId LEFT JOIN [dbo].[User] CU on CU.UserId = LB.CreatedBy";
                        gridParams.WhereClause = " IsDelete=0  " + Where;
                        gridParams.ExportedFileName = "Logbook List";
                        break;
                    }
                case "Designation":
                    {
                        gridParams.ColumnsName = "DesignationId,Name,SortOrder,IsActive,IsDelete";
                        gridParams.SortColumn = "DesignationId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "Designation";
                        gridParams.WhereClause = " IsDelete=0  " + Where;
                        gridParams.ExportedFileName = "Designation List";
                        break;
                    }
                case "Employee":
                    {
                        gridParams.ColumnsName = "EM.EmployeeId,DE.Name as Designation,EM.FirstName + ' ' + EM.LastName as Name,EM.Email,EM.Picture,EM.Mobile,EM.IsActive,EM.IsDelete";
                        gridParams.SortColumn = "EmployeeId";
                        gridParams.SortOrder = "desc";
                        gridParams.TableName = "[dbo].[Employee] EM LEFT JOIN [dbo].[Designation] DE on DE.DesignationId = EM.DesignationId";
                        gridParams.WhereClause = " IsDelete=0  " + Where;
                        gridParams.ExportedFileName = "Employee List";
                        break;
                    }
                default:
                    break;
            }
        }
    }
}