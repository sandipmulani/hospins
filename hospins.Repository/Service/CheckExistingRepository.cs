using hospins.Repository.Data;
using hospins.Repository.DataServices;
using hospins.Repository.Infrastructure;
using hospins.Repository.ServiceContract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace hospins.Repository.Services
{
    public class CheckExistingRepository : ICheckExistingRepository
    {
        private readonly hospinsContext db;

        public CheckExistingRepository(hospinsContext _db)
        {
            this.db = _db;
        }

        public string CheckChildRecordExistsForDelete(string SpName, IDictionary<string, object> _dic, string strDBConn = "")
        {
            string result = "@result";
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper(strDBConn);
                foreach (var item in _dic)
                    dbHelper.AddParameter("@" + item.Key, item.Value);
                dbHelper.AddParameter("@result", "", DatabaseHelper.ParamType.Output);
                result = dbHelper.ExecuteNonQuery(SpName, CommandType.StoredProcedure, result);
                dbHelper.Dispose();
            }
            catch (Exception ex)
            {
                ex.SetLog("Error in CheckUserLogin");
            }
            return result;
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
