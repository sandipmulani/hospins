using System;
using System.Linq;
using System.Net;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;
using hospins.Repository.Data;
using hospins.Repository.ServiceContract;
using hospins.Repository.Infrastructure;
using System.Security.Cryptography;
using System.Text;
using hospins.Repository.Models;
using hospins.Models;
using hospins.Extensions;

namespace hospins.Infrastructure
{
    public static class Common
    {
        public static bool IsBase64(this string base64String)
        {
            if (base64String.Contains(','))
                base64String = base64String.Split(',')[1];
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
                ex.SetLog("FileUpload");
            }
            return false;
        }

        public static List<SelectListItem> BindMailTemplateDDL()
        {
            List<SelectListItem> mailTemplateItems = new List<SelectListItem>
            {
                new SelectListItem { Text = "Blank Frame", Value = "Blank Frame" },
                new SelectListItem { Text = "Forgot Password", Value = "Forgot Password" },
                new SelectListItem { Text = "Account Locked - User", Value = "Account Locked - User" },
                new SelectListItem { Text = "Forgot Password - Front", Value = "Forgot Password - Front" },
                new SelectListItem { Text = "Contact Us - User - Front", Value = "Contact Us - User - Front" },
                new SelectListItem { Text = "Contact Us - Admin - Front", Value = "Contact Us - Admin - Front" }
            };

            return mailTemplateItems;
        }
        public static string GetExtension(string mimeType)
        {
            var mappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase) {
                {"image/png", ".png"},
                {"text/plain", ".txt"},
                {"image/jpg", ".jpg"},
                {"image/jpeg", ".jpeg"},
                {"image/gif", ".gif"},
                {"image/tif", ".tif"},
                {"image/tiff", ".tiff"},
                {"image/bmp", ".bmp"},
                {"application/vnd.ms-excel", ".xls"},
                {"application/octet-stream", ".xls"},
                {"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", ".xlsx"},
                {"application/vnd.oasis.opendocument.spreadsheet", ".ods"},
                {"application/msword", ".doc"},
                {"application/vnd.openxmlformats-officedocument.wordprocessingml.document", ".docx"},
                {"application/vnd.ms-powerpoint", ".ppt"},
                {"application/vnd.openxmlformats-officedocument.presentationml.presentation", ".pptx"},
                {"video/mp4", ".mp4"},
                {"video/3gpp", ".3gp"},
                {"video/mpeg", ".mpeg"},
                {"video/3gpp2", ".3gp2"},
                {"video/x-flv", ".flv"},
                {"video/x-msvideo", ".avi"},
                {"video/x-ms-wmv", ".wmv"},
                {"video/quicktime", ".mov"},
                {"application/pdf", ".pdf"},
                {"application/zip", ".zip"},
                {"text/xml", ".xml"},
                {"application/xml", ".svc"}
            };
            if (mimeType == null)
            {
                return "ERROR";
            }

            if (mimeType.StartsWith("."))
            {
                return "ERROR";
            }

            foreach (KeyValuePair<string, string> kvp in mappings)
            {
                if (string.Equals(kvp.Key, mimeType, StringComparison.OrdinalIgnoreCase))
                {
                    return kvp.Value;
                }
            }
            return "ERROR";
        }

        public static string GetIPAddresses()
        {
            StringBuilder sIpaddress = new StringBuilder();
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                if (host.AddressList != null)
                {
                    foreach (var varIP in host.AddressList)
                    {
                        if (varIP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            sIpaddress.Append(varIP.ToString()).Append(',');
                    }
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Common_427");
            }

            return sIpaddress.ToString().Trim(',');
        }

        private const int INDEX_START = 0;

        private enum CryptoType
        {
            Encrypt = 1,
            Decrypt = 2,
        }

        public static string Encrypt(string plainText, string key)
        {
            var encryptText = string.Empty;

            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] cipherTextBytes;
            ICryptoTransform encryptor = CreateCryptoByType(CryptoType.Encrypt, key);
            using (var memoryStream = new System.IO.MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, INDEX_START, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                    encryptText = Convert.ToBase64String(cipherTextBytes);
                }
                memoryStream.Close();
            }
            return encryptText;
        }

        public static string Decrypt(string encryptedText, string key)
        {
            var decryptedText = string.Empty;
            encryptedText = encryptedText.Replace(" ", "+");
            int a = 4 - (encryptedText.Length % 4);
            if (a > 0 && a < 4)
            {
                for (int i = 0; i < a; i++)
                    encryptedText += "=";
            }
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);

            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
            {
                var decryptor = CreateCryptoByType(CryptoType.Decrypt, key);
                using CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, INDEX_START, plainTextBytes.Length);
                decryptedText = Encoding.UTF8.GetString(plainTextBytes, INDEX_START, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            return decryptedText;
        }

        private static ICryptoTransform CreateCryptoByType(CryptoType cryptoType, string key)
        {
            byte[] keyBytes = Convert.FromBase64String(key);
            var rijndaelAlgorithm = new RijndaelManaged() { Mode = CipherMode.ECB };
            if (cryptoType.Equals(CryptoType.Encrypt))
            {
                rijndaelAlgorithm.Padding = PaddingMode.Zeros;
                var encryptor = rijndaelAlgorithm.CreateEncryptor(keyBytes, null);
                return encryptor;
            }
            else
            {
                rijndaelAlgorithm.Padding = PaddingMode.None;
                var decryptor = rijndaelAlgorithm.CreateDecryptor(keyBytes, null);
                return decryptor;
            }
        }

        public static bool IsValidDate<T>(IList<T> data, DateTime fromDate, DateTime toDate)
        {
            //{ x.IsDelete = true; return x; }
            var record = data.Select(x =>
            {
                // I want to aceess CustID property of param1 and pass that value to another function
                var dataFromDate = Convert.ToDateTime(typeof(T).GetProperty("FromDate").GetValue(x));
                var dataToDate = Convert.ToDateTime(typeof(T).GetProperty("ToDate").GetValue(x));
                var result = (dataFromDate <= fromDate && dataFromDate >= fromDate)
                                || (dataFromDate <= toDate && dataToDate >= toDate)
                                || (fromDate <= dataFromDate && toDate >= dataFromDate)
                                || (fromDate <= dataToDate && toDate >= dataToDate);

                return !result ? default : Convert.ToString(typeof(T).GetProperty("ToDate").GetValue(x));
            }).ToList();
            return !string.IsNullOrEmpty(record.Find(x => !string.IsNullOrEmpty(x)));
        }

        public static string GetPublicFullPath(string folderName)
        {
            return "CMSPhotoes\\" + folderName;
        }

        /// <summary>
        /// Set drop down value as string
        /// </summary>
        /// <param name="left">Left value</param>
        /// <param name="right">Right value</param>
        /// <returns>Return string value with "-"</returns>
        public static string DDStringFormat(int left, int right)
        {
            return string.Format("{0}-{1}", left.ToString(), right.ToString());
        }

        public static string GetFilterClause(List<SearchGrid> filters)
        {
            StringBuilder where = new StringBuilder();
            var sUnqField = filters.Select(t => t.FieldName).Distinct().ToList();
            foreach (var item in sUnqField)
            {
                var Sfilter = filters.Where(t => t.FieldName == item);
                where.Append(" AND ");
                where.Append(Sfilter.Count() > 1 ? "(" : "");
                var isAddOR = false;
                foreach (SearchGrid obj in Sfilter)
                {
                    if (isAddOR)
                        where.Append(" OR ");
                    switch (obj.Datatype)
                    {
                        case "contains":
                            where.Append(obj.FieldName).Append(" like '%").Append(obj.FieldValue.Replace("'", "''")).Append("%'");
                            break;
                        case "in":
                            var splitIn = obj.FieldValue.Split(',');
                            where.Append(" ( ");
                            var inIdx = 1;
                            foreach (var itemIn in splitIn)
                            {
                                where.Append(obj.FieldName).Append(" like '%").Append(itemIn.Replace("'", "''")).Append("%'");
                                if (inIdx != splitIn.Length)
                                {
                                    where.Append(" OR ");
                                }
                                inIdx++; 
                            }
                            where.Append(" ) ");
                            break;
                        case "bool":
                            where.Append(obj.FieldName).Append(" = ").Append(obj.FieldValue == "Active" ? 1 : 0);
                            break;
                        case "int":
                            where.Append(obj.FieldName).Append(" = '").Append(obj.FieldValue).Append("'");
                            break;
                        case "daterange":
                            var FieldName = obj.FieldName.Split(",");
                            var FieldValue = obj.FieldValue.Split("-");
                            where.Append("cast(").Append(FieldName[0]).Append(" as date) >= cast('").Append(FieldValue[0].Trim()).Append("' as date) and cast(").Append(FieldName[1]).Append(" as date) <= cast('").Append(FieldValue[1].Trim()).Append("' as date)");
                            break;
                    }
                    isAddOR = true;
                }
                where.Append(Sfilter.Count() > 1 ? ")" : "");
            }
            return where.ToString();
        }
    }
}
