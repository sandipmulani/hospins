using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using hospins.Repository.Infrastructure;
using OfficeOpenXml;
using hospins.Repository;

namespace hospins.Infrastructure
{
    public static class CommonExtensions
    {
        public static HttpContext HttpContextAccessor => new HttpContextAccessor().HttpContext;

        /// <summary>
        /// for render view as a string. when want to stay changes as it as before postback of page.
        /// </summary>
        /// <param name="controller"></param>
        /// <param name="viewName">name of view</param>
        /// <param name="model">model of view(if required)</param>
        /// <returns></returns>
        public static string RenderPartialViewToString(this Controller controller, string viewName, object model, IServiceProvider _service)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }
            controller.ViewData.Model = model;

            using var sw = new StringWriter();
            var _viewEngine = _service.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            ViewEngineResult viewResult = null;

            if (viewName.EndsWith(".cshtml"))
            {
                viewResult = _viewEngine.GetView(viewName, viewName, false);
            }
            else
            {
                viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, false);
            }

            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw, new HtmlHelperOptions());
            var t = viewResult.View.RenderAsync(viewContext);
            t.Wait();

            return sw.GetStringBuilder().ToString();
        }

        public static string RenderViewToString(this Controller controller, string viewName, object model, IServiceProvider _service)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = controller.ControllerContext.ActionDescriptor.ActionName;
            }
            controller.ViewData.Model = model;

            using var sw = new StringWriter();
            var _viewEngine = _service.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;
            ViewEngineResult viewResult = null;

            if (viewName.EndsWith(".cshtml"))
            {
                viewResult = _viewEngine.GetView(viewName, viewName, true);
            }
            else
            {
                viewResult = _viewEngine.FindView(controller.ControllerContext, viewName, true);
            }

            ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw, new HtmlHelperOptions());
            var t = viewResult.View.RenderAsync(viewContext);
            t.Wait();

            return sw.GetStringBuilder().ToString();
        }

        /*public static DataTable ToDataTable<T>(this List<T> items)
        {
                DataTable dataTable = new DataTable(typeof(T).Name);
        
                //Get all the properties
                PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (PropertyInfo prop in Props)
                {
                    //Defining type of data column gives proper data table 
                    var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                    //Setting column names as Property names
                    dataTable.Columns.Add(prop.Name, type);
                }
                foreach (T item in items)
                {
                   var values = new object[Props.Length];
                   for (int i = 0; i < Props.Length; i++)
                   {
                        //inserting property values to datatable rows
                        values[i] = Props[i].GetValue(item, null);
                   }
                   dataTable.Rows.Add(values);
              }
              //put a breakpoint here and check datatable
              return dataTable;
        }

        public static List<T> ConvertToList<T>(this DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                //T item = GetItem<T>(row);
                //data.Add(item);
            }
            return data;
        }
        
        public static T GetItem<T>(SqlDataReader dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower() == dr.GetName(i).ToLower())
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[i])))
                        {
                            if (pro.PropertyType.Name == "String")
                                pro.SetValue(obj, Convert.ToString(dr[i]));
                            else if (pro.PropertyType.Name == "Byte[]" && string.IsNullOrEmpty(Convert.ToString(dr[i])))
                                pro.SetValue(obj, new byte[0]);
                            else if (pro.PropertyType.Name == "Decimal")
                                pro.SetValue(obj, Convert.ToDecimal(dr[i]));
                            else
                                pro.SetValue(obj, dr[i]);
                        }
                        break;
                    }
                }
            }
            return obj;
        }*/

        public static void ExportToExcel(this DataTable dt, string FileName)
        {
            try
            {
                #region worksheet
                ExcelPackage excel = new ExcelPackage();
                var workSheet = excel.Workbook.Worksheets.Add(FileName?.Length == 0 ? "Sheet1" : FileName); // set sheet name

                //workSheet.Cells[1, 1].Value = "SrNo.";
                //workSheet.Cells[1, 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                //workSheet.Cells[1, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                //workSheet.Cells[1, 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                //workSheet.Cells[1, 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                for (var i = 0; i < dt.Columns.Count; i++)
                {
                    workSheet.Cells[1, i + 1].Value = dt.Columns[i].ColumnName.Replace("_", " ");
                    workSheet.Cells[1, i + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i + 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i + 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    workSheet.Cells[1, i + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                }

                var headerRwo = workSheet.Row(1);
                headerRwo.Style.Font.SetFromFont(new System.Drawing.Font("Calibri", 11, System.Drawing.FontStyle.Bold));

                workSheet.Row(1).Height = 30;
                int j = 2; //from 2 bcs row one is header
                foreach (DataRow obj in dt.Rows)
                {
                    //workSheet.Cells[j, 1].Value = j - 1;
                    //workSheet.Cells[j, 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //workSheet.Cells[j, 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //workSheet.Cells[j, 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    //workSheet.Cells[j, 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    for (var i = 0; i < dt.Columns.Count; i++)
                    {
                        workSheet.Cells[j, i + 1].Value = Convert.ToString(obj[dt.Columns[i].ColumnName]);
                        workSheet.Cells[j, i + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[j, i + 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[j, i + 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        workSheet.Cells[j, i + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }
                    workSheet.Row(j).Height = 30;
                    j++;
                }

                using var memoryStream = new System.IO.MemoryStream();
                HttpContextAccessor.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                HttpContextAccessor.Response.Headers.Append("content-disposition", "attachment;  filename=" + (FileName?.Length == 0 ? "Excel" : FileName) + ".xlsx");
                excel.SaveAs(memoryStream);
                HttpContextAccessor.Response.ContentLength = memoryStream.ToArray().Length;
                HttpContextAccessor.Response.Body.Write(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);
                HttpContextAccessor.Response.Body.Flush();
                HttpContextAccessor.Response.Body.Dispose();
                #endregion
            }
            catch (Exception ex)
            {
                ex.SetLog("ExportToExcel-" + FileName);
                throw;
            }
        }

        public static void ExportToWord(this DataTable dt, string FileName)
        {
            try
            {
                string html = "<table cellpadding='5' border='1' style='border-spacing:0px;'>";
                //add header row
                html += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                    html += "<th>" + dt.Columns[i].ColumnName + "</th>";
                html += "</tr>";
                //add rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<tr>";
                    for (int j = 0; j < dt.Columns.Count; j++)
                        html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                    html += "</tr>";
                }
                html += "</table>";

                if (string.IsNullOrEmpty(FileName))
                    FileName = "Word";
                byte[] htmlByte = Encoding.ASCII.GetBytes(html);

                HttpContextAccessor.Response.Headers.Add("content-disposition", "attachment;filename=" + FileName + ".doc");
                HttpContextAccessor.Response.ContentType = "application/vnd.ms-word";
                HttpContextAccessor.Response.ContentLength = htmlByte.Length;
                HttpContextAccessor.Response.Body.WriteAsync(htmlByte, 0, htmlByte.Length);
                HttpContextAccessor.Response.Body.Flush();
                HttpContextAccessor.Response.Body.Dispose();
            }
            catch (Exception ex)
            {
                ex.SetLog("ExportToWord-" + FileName);
                throw;
            }
        }

        public static void ExportToCSV(this DataTable dt, string FileName)
        {
            try
            {
                string csv = string.Empty;
                foreach (DataColumn column in dt.Columns)
                {
                    //Add the Header row for CSV file.
                    csv += column.ColumnName + ',';
                }
                //Add new line.
                csv += "\r\n";
                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        //Add the Data rows.
                        if (column.ColumnName == "Promos")
                        {
                            csv += Regex.Replace(row[column.ColumnName].ToString().Trim() + ',', @"\s+", " ");
                        }
                        else
                        {
                            csv += Regex.Replace(row[column.ColumnName].ToString().Replace(",", " ").Trim() + ',', @"\s+", " ");
                        }
                    }
                    //Add new line.
                    csv += "\r\n";
                }

                if (string.IsNullOrEmpty(FileName))
                    FileName = "CommaSeparated";
                byte[] htmlByte = Encoding.ASCII.GetBytes(csv);

                HttpContextAccessor.Response.Headers.Add("content-disposition", "attachment;filename=" + FileName + ".csv");
                HttpContextAccessor.Response.ContentType = "application/vnd.ms-excel";
                HttpContextAccessor.Response.ContentLength = htmlByte.Length;
                HttpContextAccessor.Response.Body.WriteAsync(htmlByte, 0, htmlByte.Length);
                HttpContextAccessor.Response.Body.Flush();
                HttpContextAccessor.Response.Body.Dispose();
            }
            catch (Exception ex)
            {
                ex.SetLog("ExportToCSV-" + FileName);
                throw;
            }
        }

        public static void ExportToPdf(this DataTable dt, string FileName)
        {
            try
            {
                string html = "<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'>" +
                "<head><meta http-equiv='Content-Type' content='text/html; charset=UTF-8'><meta name='viewport' content='width=device-width'><meta http-equiv='X-UA-Compatible' content='IE=edge'>" +
                "<title>SamplePdf</title></head><body><table cellpadding='5' border='1' style='border-spacing:0px;'>";
                //add header row
                html += "<tr>";
                for (int i = 0; i < dt.Columns.Count; i++)
                    html += "<th>" + dt.Columns[i].ColumnName + "</th>";
                html += "</tr>";
                //add rows
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    html += "<tr>";
                    for (int j = 0; j < dt.Columns.Count; j++)
                        html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                    html += "</tr>";
                }
                html += "</table></body></html>";

                if (string.IsNullOrEmpty(FileName))
                    FileName = "Pdf";
                byte[] htmlByte = Encoding.ASCII.GetBytes(html);

                MemoryStream stream = new MemoryStream();
                stream.Write(htmlByte, 0, htmlByte.Length);
                stream.Position = 0;
                // TextWriter textWriter = new StreamWriter(stream);
                // textWriter.WriteLine(html);
                // textWriter.Flush();
                // //byte[] htmlByte = Encoding.ASCII.GetBytes(html.ToString());
                // byte[] htmlByte = stream.ToArray();
                HttpContextAccessor.Response.Headers.Clear();
                HttpContextAccessor.Response.Headers.Add("content-disposition", "attachment;filename=" + FileName + ".pdf");
                HttpContextAccessor.Response.ContentType = "application/pdf";
                HttpContextAccessor.Response.ContentLength = stream.ToArray().Length;
                HttpContextAccessor.Response.Body.WriteAsync(stream.ToArray(), 0, stream.ToArray().Length);
                stream.Dispose();
                HttpContextAccessor.Response.Body.Flush();
                HttpContextAccessor.Response.Body.Dispose();
            }
            catch (Exception ex)
            {
                ex.SetLog("ExportToPdf-" + FileName);
                throw;
            }
        }

        public static string ConvertToJSON(this DataTable table, Boolean IsSkipTotalRow = true)
        {
            var list = new List<Dictionary<string, object>>();

            foreach (DataRow row in table.Rows)
            {
                var dict = new Dictionary<string, object>();

                foreach (DataColumn col in table.Columns)
                {
                    if (IsSkipTotalRow && !string.Equals(col.ColumnName, "totalrows", StringComparison.OrdinalIgnoreCase))
                        dict[col.ColumnName] = row[col];
                }
                list.Add(dict);
            }
            return JsonConvert.SerializeObject(list);
        }

        public static string GetJsonForDataTableJS(this DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string data = dt.ConvertToJSON();
            sb.Append("{\"data\":").AppendLine(data);
            sb.Append(",\"draw\":\"").Append(Convert.ToString(HttpContextAccessor.Request.Form["draw"])).Append('\"');
            sb.Append(",\"recordsFiltered\":\"").Append(dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()).Append('\"');
            sb.Append(",\"recordsTotal\":\"").Append(dt.Rows.Count == 0 ? "0" : dt.Rows[0]["TotalRows"].ToString()).Append("\"}");
            return sb.ToString();
        }

        public static int ToInt(this object a)
        {
            try
            {
                return Convert.ToInt32(a);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static decimal ToDecimal(this object a)
        {
            try
            {
                return Convert.ToDecimal(a);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public static string ToCurrency(this object a)
        {
            try
            {
                return Convert.ToDecimal(a).ToString("#,##.00");
            }
            catch (Exception)
            {
                return "0.00";
            }
        }

        public static string ToProperDate(this object a)
        {
            try
            {
                return Convert.ToDateTime(a).ToString("MMM dd, yyyy");
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string GetExtension(this string base64String)
        {
            string ext = ".jpeg";
            if (!string.IsNullOrEmpty(base64String) && base64String.Contains("data:image"))
            {
                ext = "." + base64String.Split(';')[0].Split('/')[1];
            }
            return ext;
        }

        public static bool IsBase64(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0
            || base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return false;
            }

            try
            {
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception ex)
            {
                ex.SetLog("CommonExtensions/IsBase64");
                // Handle the exception
            }
            return false;
        }

        public static List<T> ReadExcel<T>(this string fileName)
        {
            try
            {
                var dt = new DataTable();
                FileInfo file = new FileInfo(fileName);
                ExcelPackage excelPackage = new ExcelPackage(file);

                ExcelWorksheet sheet = excelPackage.Workbook.Worksheets[1];
                foreach (var col in sheet.Cells[1, 1, 1, sheet.Dimension.End.Column])
                {
                    dt.Columns.Add(col.Text);
                }
                for (int rowNum = 2; rowNum <= sheet.Dimension.End.Row; rowNum++)
                {
                    var wsRow = sheet.Cells[rowNum, 1, rowNum, sheet.Dimension.End.Column];
                    DataRow row = dt.NewRow();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    dt.Rows.Add(row);
                }
                return CommonFunctions.ConvertDataTable<T>(dt);
            }
            catch //(Exception ex)
            {
                throw; //ex;
            }
        }

        public static List<int> GetPresents(this List<int> _obj1, List<int> _obj2)
        {
            var _res = new List<int>();
            foreach (var item in _obj1)
            {
                if (_obj2.Contains(item))
                {
                    _res.Add(item);
                }
            }
            return _res;
        }

        public static string? UploadFiles(IFormFile file)
        {
            // Checking no of files injected in Request object  
            if (file != null)
            {
                try
                {
                    //  Get all files from Request object
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/EmployeeDoc");

                    //create folder if not exist
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    //get file extension
                    var fileInfo = new FileInfo(file.FileName);
                    string fileName = file.FileName + "_" + Guid.NewGuid() + fileInfo.Extension;
                    string fileNameWithPath = Path.Combine(path, fileName);
                    using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    // Returns message that successfully uploaded  
                    return fileName;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
    }
}
