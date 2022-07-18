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
    public class UserRepository : IUserRepository
    {
        private readonly hospinsContext db;

        public UserRepository(hospinsContext _db)
        {
            this.db = _db;
        }

        public User CheckLogin(string username, string password)
        {
            try
            {
                string strEncrPass = SecurityLibrary.GetEncryptedString(password);
                return this.db.Users.Where(x => x.Username == username && x.Password == strEncrPass && x.UserTypeId == (int)EnmUserType.User && x.IsActive && !x.IsDelete).FirstOrDefault();
            }
            catch (Exception ex)
            {
                ex.SetLog("CheckLogin_Repository");
                throw;
            }
        }

        public List<User> GetAll(Expression<Func<User, bool>> expression = null)
        {
            if (expression == null)
                return db.Users.ToList();
            else
                return db.Users.Where(expression).ToList();
        }

        public User GetById(int Id)
        {
            return db.Users.Find(Id);
        }

        public int Insert(User obj, int UserId)
        {
            try
            {
                obj.Password = SecurityLibrary.GetEncryptedString(obj.Password);
                obj.IsActive = true;
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = UserId;
                db.Users.Add(obj);
                db.SaveChanges(UserId);
                return obj.UserId;
            }
            catch (Exception ex)
            {
                ex.SetLog("Insert User Repository");
                return -1;
            }
        }

        public void Update(User obj, int UserId, string mode = "", string newpassword = "")
        {
            try
            {
                obj.ModifyBy = UserId;
                obj.ModifyDate = DateTime.Now;
                obj.IsActive = mode == "status" ? obj.IsActive : mode != "delete";
                if (newpassword != null && newpassword.Trim() != "")
                {
                    obj.Password = SecurityLibrary.GetEncryptedString(newpassword);
                }
                else
                {
                    db.Entry(obj).Property(x => x.Password).IsModified = false;
                }
                db.Entry(obj).State = EntityState.Modified;
                db.Entry(obj).Property(x => x.Password).IsModified = false;
                db.Entry(obj).Property(x => x.CreatedBy).IsModified = false;
                db.Entry(obj).Property(x => x.CreatedDate).IsModified = false;
                db.SaveChanges(UserId, mode);
            }
            catch (Exception ex)
            {
                ex.SetLog("Update User Repository");
            }
        }

        public void ChangePasswordUpdate(User obj, string mode = "")
        {
            try
            {
                db.Entry(obj).State = EntityState.Modified;
                db.SaveChanges(obj.UserId, mode);
            }
            catch (Exception ex)
            {
                ex.SetLog("Update User Repository");
            }
        }

        /// <summary>
        /// Remove multiple data from database
        /// </summary>
        /// <param name="obj">list of obj</param>
        public void MultiDelete(List<User> obj, int UserId)
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
                ex.SetLog("Update User Multi Delete");
            }
        }

        public void UserProfileUpdate(User obj, string mode = "")
        {
            try
            {
                db.Entry(obj).State = EntityState.Modified;
                db.Entry(obj).Property(e => e.Username).IsModified = false;
                db.Entry(obj).Property(e => e.Password).IsModified = false;
                db.Entry(obj).Property(e => e.IsActive).IsModified = false;
                db.Entry(obj).Property(e => e.IsDelete).IsModified = false;
                db.Entry(obj).Property(e => e.CreatedBy).IsModified = false;
                db.Entry(obj).Property(e => e.CreatedDate).IsModified = false;
                db.SaveChanges(obj.UserId, mode);
            }
            catch (Exception ex)
            {
                ex.SetLog("Update User Repository");
            }
        }

        public string CheckUserExist(int UserId, string Email, string strDBConn)
        {
            const string outputParam = "@EmailCount";
            string result = string.Empty;
            try
            {
                DatabaseHelper dbHelper = new DatabaseHelper(strDBConn);
                dbHelper.AddParameter("@Id", UserId);
                dbHelper.AddParameter("@Email", Email);
                dbHelper.AddParameter("@EmailCount", "", DatabaseHelper.ParamType.Output);

                result = dbHelper.ExecuteNonQuery("CheckUserExist", CommandType.StoredProcedure, outputParam);
            }
            catch (Exception ex)
            {
                ex.SetLog("UserRepository/CheckUserExist");
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
