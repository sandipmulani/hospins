using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using hospins.Repository.Data;
using hospins.Repository.Models;
using hospins.Repository.Infrastructure;
using hospins.Repository.ServiceContract;
using System.Data;
using System.Data.SqlClient;
using hospins.Repository.DataServices;

namespace hospins.Repository.Services
{
    public class CommonRepository<T> : ICommonRepository<T> where T : class //, IDisposable - in this case need to inherit IDisposable For all T
    {
        protected readonly hospinsContext db;

        public CommonRepository(hospinsContext _db)
        {
            db = _db;
        }

        /// <summary>
        /// Get all data with/without where clause
        /// </summary>
        /// <param name="expression">where clause</param>
        /// <returns>list of items</returns>
        public List<T> GetAll(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
                return db.Set<T>().ToList();
            else
                return db.Set<T>().Where(expression).ToList();
        }

        /// <summary>
        /// Get all data with/without where clause
        /// </summary>
        /// <param name="expression">where clause</param>
        /// <returns>list of items</returns>
        public List<T> GetAll(int firstNRow, Expression<Func<T, object>> order, Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
                return db.Set<T>().OrderBy(order).Take(firstNRow).ToList();
            else
                return db.Set<T>().Where(expression).OrderBy(order).Take(firstNRow).ToList();
        }

        /// <summary>
        /// Get all data with/without where clause and get child record if need
        /// </summary>
        /// <param name="expression">where clause</param>
        /// <param name="orderBy">order by clause</param>
        /// <param name="includeProperties">include</param>
        /// <returns>list of items</returns>
        public IEnumerable<T> Get(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = db.Set<T>().Where(expression);

            if (expression != null)
            {
                query = query.Where(expression);
            }

            foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            return orderBy != null ? orderBy(query).ToList() : query.ToList();
        }

        /// <summary>
        /// Get item by int id
        /// </summary>
        /// <param name="id">primary key value</param>
        /// <returns>object of class</returns>
        public T GetById(int id)
        {
            return db.Set<T>().Find(id);
        }

        /// <summary>
        /// Get item by id with no tracking
        /// </summary>
        /// <param name="id">primary key value</param>
        /// <returns>object of class</returns>
        public T GetByIdWithNoTracking(int id)
        {
            var entity = db.Set<T>().Find(id);
            if (entity != null)
            {
                db.Entry(entity).State = EntityState.Detached;
            }
            return entity;
        }

        /// <summary>
        /// Get item by string id
        /// </summary>
        /// <param name="id">primary key value</param>
        /// <returns>object of class</returns>
        public T GetById(string Id)
        {
            return db.Set<T>().Find(Id);
        }

        /// <summary>
        /// Get top 1 item
        /// </summary>
        /// <returns>object of class</returns>
        public T GetTop1()
        {
            return db.Set<T>().FirstOrDefault();
        }

        /// <summary>
        /// Insert data to database
        /// </summary>
        /// <param name="obj">object to be insert</param>
        public void Insert(T obj, int UserId)
        {
            try
            {
                db.Set<T>().Add(obj);
                db.SaveChanges(UserId);
            }
            catch (Exception ex)
            {
                ex.SetLog("Insert Common Repository");
            }
        }

        /// <summary>
        /// Insert list of data
        /// </summary>
        /// <param name="obj">object list</param>
        /// <param name="UserId">user id</param>
        public void InsertRange(List<T> obj, int UserId)
        {
            try
            {
                db.Set<T>().AddRange(obj);
                db.SaveChanges(UserId);
            }
            catch (Exception ex)
            {
                ex.SetLog("Insert Common Repository");
            }
        }

        /// <summary>
        /// Insert list of data
        /// </summary>
        /// <param name="obj">object list</param>
        /// <param name="UserId">user id</param>
        /// <returns>Returns list of generic type</returns>
        public List<T> InsertRangeAndGetObj(List<T> obj, int UserId)
        {
            try
            {
                db.Set<T>().AddRange(obj);
                db.SaveChanges(UserId);
            }
            catch (Exception ex)
            {
                ex.SetLog("Insert Common Repository");
            }
            return obj;
        }

        /// <summary>
        /// Insert data to database and get inserted data as object
        /// </summary>
        /// <param name="obj">object to be insert</param>
        public T InsertAndGetObj(T obj, int UserId)
        {
            try
            {
                db.Set<T>().Add(obj);
                db.SaveChanges(UserId);
                return obj;
            }
            catch (Exception ex)
            {
                ex.SetLog("Insert Common Repository");
                return obj;
            }
        }

        /// <summary>
        /// Insert list of data to database
        /// </summary>
        /// <param name="obj">object to be insert</param>
        public void InsertList(List<T> objList, int UserId)
        {
            try
            {
                if (objList?.Count > 0)
                {
                    foreach (var item in objList)
                    {
                        db.Set<T>().Add(item);
                    }
                    db.SaveChanges(UserId);
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Insert Common Repository");
            }
        }

        /// <summary>
        /// Update data to database 
        /// </summary>
        /// <param name="obj">object to be update</param>
        /// <param name="mode">if it's delete then audit log will be set to delete entry.</param>
        public void Update(T obj, int UserId, string mode = "update")
        {
            try
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges(UserId, mode);
            }
            catch (Exception ex)
            {
                ex.SetLog("Update Common Repository");
            }
        }

        /// <summary>
        /// Update data to database and get updated data as object
        /// </summary>
        /// <param name="obj">object to be update</param>
        /// <param name="mode">if it's delete then audit log will be set to delete entry.</param>
        public T UpdateAndGetObj(T obj, int UserId, string mode = "update")
        {
            try
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges(UserId, mode);
                return obj;
            }
            catch (Exception ex)
            {
                ex.SetLog("Update Common Repository");
                return obj;
            }
        }

        /// <summary>
        /// Remove data from database
        /// </summary>
        /// <param name="id">primaru key value</param>
        public void Delete(int id, int UserId)
        {
            try
            {
                T obj = db.Set<T>().Find(id);
                if (obj != null)
                {
                    db.Set<T>().Remove(obj);
                    db.SaveChanges(UserId, "delete");
                }
            }
            catch (Exception ex)
            {
                ex.SetLog("Delete Common Repository");
            }
        }

        /// <summary>
        /// Remove multiple data from database
        /// </summary>
        /// <param name="obj">list of obj</param>
        public void MultiDelete(List<T> obj, int UserId)
        {
            try
            {
                foreach (var item in obj)
                {
                    db.Entry(item).State = EntityState.Modified;
                }
                db.SaveChanges(UserId, "delete");
            }
            catch (Exception ex)
            {
                ex.SetLog("Update Common Repository");
            }
        }

        /// <summary>
        /// Remove list of data from database
        /// </summary>
        /// <param name="id">object list</param>
        public void DeleteRange(List<T> obj, int UserId)
        {
            try
            {
                db.Set<T>().RemoveRange(obj);
                db.SaveChanges(UserId, "delete");
            }
            catch (Exception ex)
            {
                ex.SetLog("Delete Common Repository");
            }
        }

        /// <summary>
        /// Remove list from database
        /// </summary>
        /// <param name="ob">configurations</param>
        /// <param name="strDBConn">Database connection string</param>
        public DataTable GetDataList(CommonColumn ob, string strDBConn = "")
        {
            DatabaseHelper dbHelper = new DatabaseHelper(strDBConn);
            dbHelper.AddParameter("@TableName", ob.TableName);
            dbHelper.AddParameter("@ColumnsName", ob.ColumnsName);
            dbHelper.AddParameter("@SortOrder", ob.SortOrder);
            dbHelper.AddParameter("@SortColumn", ob.SortColumn);
            dbHelper.AddParameter("@PageNumber", ob.PageNumber);
            dbHelper.AddParameter("@RecordPerPage", ob.RecordPerPage);
            dbHelper.AddParameter("@WhereClause", ob.WhereClause);

            DataSet ds = dbHelper.ExecuteDataSet("GetDataForGridWeb", CommandType.StoredProcedure);
            DataTable dt = ds.Tables[0];
            dbHelper.Dispose();
            return dt;
        }

        public DataTable GetDataList(string sp_Name, SqlParameter[] param, string strDBConn = "")
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper(strDBConn);
                DataTable result = dbHelper.FetchDataTableBySP(sp_Name, param);
                dbHelper.Dispose();
                return result;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public int DeleteRecordSproc(string sp_Name, SqlParameter[] param, string strDBConn = "")
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper(strDBConn);
                var result = dbHelper.ExecuteNonQuery(sp_Name, param);
                dbHelper.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string GetDecryptValue(byte[] encryptData, string strDBConn)
        {
            string result = string.Empty;
            try
            {
                result = "@DecryptText";
                DatabaseHelper dbHelper = new DatabaseHelper(strDBConn);
                dbHelper.AddParameter("@EncryptText", encryptData);
                dbHelper.AddParameter("@DecryptText", "", DatabaseHelper.ParamType.Output);
                result = dbHelper.ExecuteNonQuery("GetDecryptValue", CommandType.StoredProcedure, result);
                dbHelper.Dispose();
            }
            catch (Exception ex)
            {
                ex.SetLog("Error in Decrypt Value");
            }
            return result;
        }

        public List<T> GetBySp<T>(string sp_Name, SqlParameter[] param, string strDBConn = "")
        {
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper(strDBConn);
                var result = dbHelper.FetchListBySP<T>(sp_Name, param);
                dbHelper.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                ex.SetLog("Get ,Repository");
                throw;
            }
        }

        #region IDisposable Support

        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    db.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
