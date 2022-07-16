using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using hospins.Repository.Models;

namespace hospins.Repository.ServiceContract
{
    public interface ICommonRepository<T> : IDisposable where T : class
    {
        /// <summary>
        /// Get all data with/without where clause
        /// </summary>
        /// <param name="expression">where clause</param>
        /// <returns>list of items</returns>
        List<T> GetAll(Expression<Func<T, bool>> expression = null);

        List<T> GetAll(int firstNRow, Expression<Func<T, object>> order, Expression<Func<T, bool>> expression = null);

        IEnumerable<T> Get(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        /// <summary>
        /// Get item by int id
        /// </summary>
        /// <param name="id">primary key value</param>
        /// <returns>object of class</returns>
        T GetById(int id);
        T GetByIdWithNoTracking(int Id);

        /// <summary>
        /// Get item by string id
        /// </summary>
        /// <param name="id">primary key value</param>
        /// <returns>object of class</returns>
        T GetById(string Id);

        /// <summary>
        /// Get top 1 item
        /// </summary>
        /// <returns>object of class</returns>
        T GetTop1();

        /// <summary>
        /// Insert data to database
        /// </summary>
        /// <param name="obj">object to be insert</param>
        void Insert(T obj, int UserId);

        /// <summary>
        /// Insert data to database and get inserted data as object
        /// </summary>
        /// <param name="obj">object to be insert</param>
        T InsertAndGetObj(T obj, int UserId);

        /// <summary>
        /// Insert list of data to database
        /// </summary>
        /// <param name="obj">object to be insert</param>
        void InsertList(List<T> objList, int UserId);

        /// <summary>
        /// Update data to database
        /// </summary>
        /// <param name="obj">object to be update</param>
        /// <param name="mode">if it's delete then audit log will be set to delete entry.</param>
        void Update(T obj, int UserId, string mode = "update");

        T UpdateAndGetObj(T obj, int UserId, string mode = "update");

        /// <summary>
        /// Remove data from database
        /// </summary>
        /// <param name="id">primaru key value</param>
        void Delete(int id, int UserId);

        void MultiDelete(List<T> obj, int UserId);

        /// <summary>
        /// Insert list of data
        /// </summary>
        /// <param name="obj">object list</param>
        /// <param name="UserId">user id</param>
        void InsertRange(List<T> obj, int UserId);

        /// <summary>
        /// Insert list of data
        /// </summary>
        /// <param name="obj">object list</param>
        /// <param name="UserId">user id</param>
        /// <returns>Returns list of generic type</returns>
        List<T> InsertRangeAndGetObj(List<T> obj, int UserId);

        /// <summary>
        /// Remove list of data from database
        /// </summary>
        /// <param name="id">object list</param>
        void DeleteRange(List<T> obj, int UserId);

        /// <summary>
        /// Remove list from database
        /// </summary>
        /// <param name="ob">configurations</param>
        /// <param name="strDBConn">Database connection string</param>
        DataTable GetDataList(CommonColumn ob, string strDBConn = "");

        DataTable GetDataList(string sp_Name, SqlParameter[] param, string strDBConn = "");

        //string InsertXML(InsertUpdateSP obj, string strDBConnLog = "");

        string GetDecryptValue(byte[] encryptData, string strDBConn);
        
        List<T> GetBySp<T>(string sp_Name, SqlParameter[] param, string strDBConn = "");

        int DeleteRecordSproc(string sp_Name, SqlParameter[] param, string strDBConn = "");
    }
}
