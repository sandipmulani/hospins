using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using hospins.Repository.Infrastructure;
using hospins.Repository.Data;
using Microsoft.AspNetCore.Http;

namespace hospins.Repository
{
    public static class CommonFunctions
    {
        public static bool ToBool(this object a)
        {
            try
            {
                return Convert.ToBoolean(a);
            }
            catch (Exception)
            {
                return true;
            }
        }

        public static T GetListItem<T>(SqlDataReader dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name.ToLower().Split('_')[0] == dr.GetName(i).ToLower().Split('_')[0])
                    {
                        if (!string.IsNullOrEmpty(Convert.ToString(dr[i])))
                        {
                            if (pro.PropertyType.Name == "String")
                                pro.SetValue(obj, Convert.ToString(dr[i]));
                            else if (pro.PropertyType.Name == "Byte[]" && string.IsNullOrEmpty(Convert.ToString(dr[i])))
                                pro.SetValue(obj, new byte[0]);
                            else
                                pro.SetValue(obj, dr[i]);
                        }
                        else
                        {
                            if (pro.PropertyType.Name == "String")
                                pro.SetValue(obj, "");
                        }
                        break;
                    }
                }
            }
            return obj;
        }

        public static List<T> ConvertDataTable<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                T item = GetItemNew<T>(row);
                data.Add(item);
            }
            return data;
        }

        public static T GetItemNew<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();
            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    object value = dr[column.ColumnName];
                    if (value != DBNull.Value)
                    {
                        if (string.Equals(pro.Name, column.ColumnName, StringComparison.OrdinalIgnoreCase))
                        {
                            pro.SetValue(obj, dr[column.ColumnName], null);
                        }
                        //pro.SetValue(obj, dr[column.ColumnName] ?? "", null);  
                        else
                        {
                            continue;
                        }
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// Find the missing number in a randomly-sorted array
        /// </summary>
        /// <param name="numbers">Array of random numbers between <paramref name="min"/> and <paramref name="max"/> (inclusive) with one number missing</param>
        /// <param name="min">Minimum number (inclusive)</param>
        /// <param name="max">Maximum number (inclusive)</param>
        /// <returns>Missing number</returns>
        public static int MissingNumber(this List<int> myList)
        {
            int a = myList.OrderBy(x => x).First();
            int b = myList.OrderBy(x => x).Last();

            List<int> myList2 = Enumerable.Range(a, b - a + 1).ToList();
            List<int> remaining = myList2.Except(myList).ToList();
            if (remaining.Count == 0) return b + 1;
            return remaining.Min();
        }

        private static int CalculateAge(DateTime dateOfBirth)  
        {  
            int age = 0;  
            age = DateTime.Now.Year - dateOfBirth.Year;  
            if (DateTime.Now.DayOfYear < dateOfBirth.DayOfYear)  
                age = age - 1;  
        
            return age;  
        }  
        
        public static int ConvertToInt(this object a)
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
    }
}
