using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;
using hospins.Repository.DataServices;
using hospins.Repository.Infrastructure;

namespace hospins.Infrastructure
{
    /// <summary>
    /// for configure all dropdowns of system.
    /// </summary>
    public class DropdownConfig
    {
        public string Mode { get; set; }
        private string Text { get; }
        private string Value { get; }
        private string Where { get; }
        private int Total { get; }
        private string Keyword { get; }
        private string Table { get; }
        private string Order { get; }

        /// <summary>
        /// init dropdown configuration
        /// </summary>
        /// <param name="mode">keyword for identify grid</param>
        public DropdownConfig(string mode, string keyword, int total = 20, string where = "")
        {
            this.Mode = mode;
            this.Total = total;
            this.Keyword = keyword;
            if (string.IsNullOrEmpty(where)) where = "1=1";
            switch (true)
            {
                case bool _ when string.Equals(mode, "role", StringComparison.OrdinalIgnoreCase):

                    {
                        this.Text = "RoleName";
                        this.Value = "RoleId";
                        this.Where = "IsActive=1 AND RoleId > 1";
                        this.Table = "Role";
                        this.Order = "RoleName asc";
                        break;
                    }
                case bool _ when string.Equals(mode, "user", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Username";
                        this.Value = "UserId";
                        this.Where = "IsActive=1";
                        this.Table = "[User]";
                        this.Order = "Username asc";
                        break;
                    }
                case bool _ when string.Equals(mode, "Category", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "CategoryId";
                        this.Where = "IsActive=1 AND IsDelete = 0";
                        this.Table = "Category";
                        break;
                    }
                case bool _ when string.Equals(mode, "SubCategory", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "SubCategoryId";
                        this.Where = "IsActive=1 AND IsDelete = 0";
                        this.Table = "SubCategory";
                        break;
                    }
                case bool _ when string.Equals(mode, "Priority", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "PriorityId";
                        this.Where = "IsActive=1 AND IsDelete = 0";
                        this.Table = "Priority";
                        break;
                    }
                case bool _ when string.Equals(mode, "Designation", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "DesignationId";
                        this.Where = "IsActive=1 AND IsDelete = 0";
                        this.Table = "Designation";
                        break;
                    }
                case bool _ when string.Equals(mode, "Country", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "CountryId";
                        this.Where = "1=1";
                        this.Table = "Country";
                        break;
                    }
                case bool _ when string.Equals(mode, "AddressType", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "AddressTypeId";
                        this.Where = "IsActive=1 AND IsDelete = 0";
                        this.Table = "AddressType";
                        break;
                    }
                case bool _ when string.Equals(mode, "SalaryType", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "SalaryTypeId";
                        this.Where = "IsActive=1 AND IsDelete = 0";
                        this.Table = "SalaryType";
                        break;
                    }
                case bool _ when string.Equals(mode, "DocumentType", StringComparison.OrdinalIgnoreCase):
                    {
                        this.Text = "Name";
                        this.Value = "DocumentTypeId";
                        this.Where = "IsActive=1 AND IsDelete = 0";
                        this.Table = "DocumentType";
                        break;
                    }
                default:
                    break;
            }
        }

        public List<SelectListItem> GetList(string cn, string IsDefault)
        {
            try
            {
                DataTable dt = GetData(cn);
                List<SelectListItem> lst = new List<SelectListItem>();
                if (!string.IsNullOrWhiteSpace(IsDefault))
                    lst.Add(new SelectListItem() { Value = "", Text = IsDefault });
                foreach (DataRow dr in dt.Rows)
                {
                    lst.Add(new SelectListItem() { Value = Convert.ToString(dr["Value"]), Text = Convert.ToString(dr["text"]) });
                }
                return lst;
            }
            catch (Exception ex)
            {
                ex.SetLog("DropdownBind");
                throw;
            }
        }

        private DataTable GetData(string cn)
        {
            DatabaseHelper dbHelper = new DatabaseHelper(cn);
            SqlParameter[] para = new SqlParameter[7];
            para[0] = new SqlParameter("@Text", this.Text);
            para[1] = new SqlParameter("@TableName", this.Table);
            para[2] = new SqlParameter("@Value", this.Value);
            para[3] = new SqlParameter("@Where", this.Where);
            para[4] = new SqlParameter("@Count", this.Total);
            para[5] = new SqlParameter("@Keyword", this.Keyword);
            para[6] = new SqlParameter("@Order", this.Order);
            DataTable dt = dbHelper.FetchDataTableBySP("GetDataForDropdown", para);
            dbHelper.Dispose();
            return dt;
        }
    }
}