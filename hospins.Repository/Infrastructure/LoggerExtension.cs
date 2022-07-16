using System;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace hospins.Repository.Infrastructure
{
    public static class LoggerExtension
    {
        public static HttpContext HttpContextAccessor => new HttpContextAccessor().HttpContext;

        public static void SetLog(this Exception ex)
        {
            ex.SetLog("");
        }

        public static void SetLog(this Exception ex, string msg)
        {
            string desc = "";
            if (ex.InnerException != null)
            {
                Exception innerex = ex.InnerException;
                desc += "InnerMessage:" + innerex.Message + ",InnerSource:" + innerex.Source + ",InnerStackTrace:" + innerex.StackTrace;
            }

            if (!string.IsNullOrEmpty(desc))
                desc += ",";

            desc += "Message:" + ex.Message + ",Source:" + ex.Source + ",StackTrace:" + ex.StackTrace;

            if (!string.IsNullOrEmpty(msg))
                desc += ",Additional Message:" + msg;

            WriteToFile(desc);

            /*try
            {
                //we need to save in DB //must not go infinite due to db error and try catch
                IAuditTrail_Repository _auditTrail_Repository = _httpContextAccessor.RequestServices.GetService(typeof(IAuditTrail_Repository)) as IAuditTrail_Repository;
                _auditTrail_Repository.SaveLog((int)enmAuditAction.Add, "Exception", "0", desc);
            }
            catch {
                WriteToFile(desc);
            }*/
        }

        private static void WriteToFile(string strLine)
        {
            try
            {
                string FileName = DateTime.Now.Day + "_" + DateTime.Now.Month + "_" + DateTime.Now.Year;
                if (!Directory.Exists("Logs"))
                    Directory.CreateDirectory("Logs");
                if (!File.Exists("Logs/" + FileName + ".txt"))
                {
                    using StreamWriter sw = File.CreateText("Logs/" + FileName + ".txt");
                    sw.WriteLine(strLine);
                    sw.WriteLine("");
                    sw.Flush();
                    sw.Close();
                }
                else
                {
                    using StreamWriter sw = File.AppendText("Logs/" + FileName + ".txt");
                    sw.WriteLine(strLine);
                    sw.WriteLine("");
                    sw.Flush();
                    sw.Close();
                }
            }
            catch { }
        }
    }
}
