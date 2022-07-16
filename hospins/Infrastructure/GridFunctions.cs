using System;
using System.Data;
using System.Linq;
using hospins.Repository.DataServices;
using Microsoft.AspNetCore.Http;

namespace hospins.Infrastructure
{
    public class GridFunctions
    {
        private readonly HttpContext _HttpContext;

        public GridFunctions(HttpContext httpContext){
            _HttpContext = httpContext;
        }

        /// <summary>
        /// for get the column names
        /// </summary>
        /// <returns></returns>
        public string GetColumns()
        {
            string columns = "";
            for (int i = 0; ; i++)
            {
                if (!string.IsNullOrEmpty(_HttpContext.Request.Form["columns[" + i + "][data]"]))
                {
                    string c = Convert.ToString(_HttpContext.Request.Form["columns[" + i + "][data]"]);
                    columns += columns?.Length == 0 ? c : "," + c;
                }
                else
                {
                    break;
                }
            }
            if (!string.IsNullOrEmpty(_HttpContext.Request.Form["Columns"]))
                columns = Convert.ToString(_HttpContext.Request.Form["Columns"]);
            return columns;
        }

        /// <summary>
        /// for get sorted column name
        /// </summary>
        /// <param name="defaultColName"></param>
        /// <returns></returns>
        public string GetSortColumn(string defaultColName)
        {
            if (!string.IsNullOrEmpty(_HttpContext.Request.Form["order[0][column]"]))
            {
                string index = Convert.ToString(_HttpContext.Request.Form["order[0][column]"]);
                string ColName = Convert.ToString(_HttpContext.Request.Form["columns[" + index + "][data]"]);
                if (string.IsNullOrEmpty(ColName))
                {
                    if (index.ToInt() > 0)
                    {
                        string[] columns = Convert.ToString(_HttpContext.Request.Form["columns"]).Split(',');
                        if (columns.Length > index.ToInt())
                            ColName = columns[index.ToInt()].Split(" [")[0];
                        else
                            ColName = defaultColName;
                    }
                    else
                    {
                        ColName = defaultColName;
                    }
                }
                if(_HttpContext.Request.Form["mode"].ToString().ToLower() == "audit" && ColName == "ChangeDate") {
                    ColName = "ChangeDate1";
                }
                return ColName;
            }
            else
            {
                return defaultColName;
            }
        }

        /// <summary>
        /// for get order of sorted column
        /// </summary>
        /// <returns></returns>
        public string GetSortOrder()
        {
            if (!string.IsNullOrEmpty(_HttpContext.Request.Form["order[0][dir]"]))
            {
                string order = Convert.ToString(_HttpContext.Request.Form["order[0][dir]"]);
                if (string.IsNullOrEmpty(order))
                    order = "asc";
                return order;
            }
            else
            {
                return "asc";
            }
        }

        /// <summary>
        /// for set where clause(global serach)
        /// </summary>
        /// <param name="w"></param>
        /// <returns></returns>
        public string GetWhereClause(string w = "")
        {
            string where = w;
            if (!string.IsNullOrEmpty(_HttpContext.Request.Form["FixClause"]))
            {
                string fix = Convert.ToString(_HttpContext.Request.Form["FixClause"]);

                //'%'%' To '%''%' | '%te'st%' To '%te''st%'
                const string pattern = @"(%)(\w*)(')(\w*)(%)";
                fix = System.Text.RegularExpressions.Regex.Replace(fix, pattern, "$1$2''$4$5");

                if (fix != "")
                    where += where?.Length == 0 ? fix : " AND (" +fix + ")";
            }
            if (!string.IsNullOrEmpty(_HttpContext.Request.Form["search[value]"]))
            {
                string val = Convert.ToString(_HttpContext.Request.Form["search[value]"]);
                if (val != "")
                {
                    val = val.Replace("'", "''");
                    string whereforall = "";
                    // where = where == "" ? " 1 = 1 " : where;
                    string[] columns = GetColumns().Split(',');
                    foreach (string col in columns)
                    {
                        if (!string.Equals(col, "rownumber", StringComparison.OrdinalIgnoreCase))
                            whereforall += whereforall?.Length == 0 ? col + " LIKE N'%" + val + "%'" : " OR " + col + " LIKE N'%" + val + "%'";
                    }
                    where += where?.Length == 0 ? "(" + whereforall + ")" : " AND (" + whereforall + ")";
                }
            }
            return where;
        }

        /// <summary>
        /// for get current page number
        /// </summary>
        /// <returns></returns>
        public int GetPageNumber()
        {
            if (!string.IsNullOrEmpty(_HttpContext.Request.Form["start"]))
                return Convert.ToString(_HttpContext.Request.Form["start"])?.Length == 0 ? -1 : Convert.ToInt32(Convert.ToString(_HttpContext.Request.Form["start"]));
            else
                return 1;
        }

        /// <summary>
        /// for get total rows per page
        /// </summary>
        /// <returns></returns>
        public int GetRecordPerPage()
        {
            if (!string.IsNullOrEmpty(_HttpContext.Request.Form["length"]))
                return Convert.ToInt32(Convert.ToString(_HttpContext.Request.Form["length"]));
            else
                return 10;
        }

        /// <summary>
        /// get data
        /// </summary>
        /// <param name="oGrid"></param>
        /// <returns></returns>
        public DataTable GetDataTable(GridParams oGrid)
        {
            DatabaseHelper dbHelper = new DatabaseHelper(ConfigurationSettings.DBConnection);
            dbHelper.AddParameter("@TableName", oGrid.TableName);
            dbHelper.AddParameter("@ColumnsName", oGrid.ColumnsName);
            dbHelper.AddParameter("@SortOrder", GetSortOrder());
            dbHelper.AddParameter("@SortColumn", GetSortColumn(oGrid.SortColumn));
            dbHelper.AddParameter("@PageNumber", GetPageNumber());
            dbHelper.AddParameter("@RecordPerPage", GetRecordPerPage());
            dbHelper.AddParameter("@WhereClause", GetWhereClause(oGrid.WhereClause));
            DataSet ds = dbHelper.ExecuteDataSet("GetDataForGridWeb", CommandType.StoredProcedure);
            dbHelper.Dispose();
            return ds.Tables[0];
        }

        /// <summary>
        /// return json as a datatable response
        /// </summary>
        /// <param name="oGrid"></param>
        /// <returns></returns>
        public string GetJson(GridParams oGrid)
        {
            DataTable dt = GetDataTable(oGrid);
            return dt.GetJsonForDataTableJS();
        }

        /// <summary>
        /// export data to specific format
        /// </summary>
        /// <param name="oGrid"></param>
        public void Export(GridParams oGrid)
        {
            DataTable dt = GetDataTable(oGrid);

            oGrid.ExportedColumns = Convert.ToString(_HttpContext.Request.Form["Columns"]).Replace("null,", "");
            if (oGrid != null)
            {
                string[] c = oGrid.ExportedColumns.Split(',');
                string[] s = oGrid.ExportedColumns.Split(',');
                for (int i = 0; i < c.Length; i++)
                {
                    c[i] = c[i].Split(' ')[0];
                }

                DataTable dtTemp = dt.Copy();
                int j = 0;
                foreach (DataColumn dc in dtTemp.Columns)
                {
                    if (!c.Contains(dc.ColumnName))
                    {
                        dt.Columns.Remove(dc.ColumnName);
                    }
                    else
                    {
                        dt.Columns[s[j].Split(' ')[0]].SetOrdinal(j);
                        dt.Columns[s[j].Split(' ')[0]].ColumnName = s[j].Split('[').Length > 1 ? s[j].Split('[')[1].Replace("]", "") : s[j];
                        j++;
                    }
                }
            }
            if (dt.Columns.Contains("TotalRows"))
                dt.Columns.Remove("TotalRows");
            // if (dt.Columns.Contains("RowNumber"))
            //dt.TableName = dt.TableName +  "SR#";
            //dt.Columns["RowNumber"].ColumnName = oGrid.TableName +  "SR#";
            //dt.Columns.Remove("RowNumber");
            string type = Convert.ToString(_HttpContext.Request.Form["type"]);

            if (string.Equals(type, "excel", StringComparison.OrdinalIgnoreCase))
            {
                dt.ExportToExcel(oGrid.ExportedFileName);
            }
            else if (string.Equals(type, "word", StringComparison.OrdinalIgnoreCase))
            {
                dt.ExportToWord(oGrid.ExportedFileName);
            }
            else if (string.Equals(type, "pdf", StringComparison.OrdinalIgnoreCase))
            {
                dt.ExportToPdf(oGrid.ExportedFileName);
            }
            else
            {
                dt.ExportToExcel(oGrid.ExportedFileName);
            }
        }
    }
}