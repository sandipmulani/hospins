using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using hospins.Repository.Data;
namespace hospins.Repository.ServiceContract
{
    public interface IUserRepository : IDisposable
    {
        User CheckLogin(string username, string password,int loginType);
        List<User> GetAll(Expression<Func<User, bool>> expression = null);
        User GetById(int Id);
        int Insert(User obj, int UserId);
        void Update(User obj, int UserId, string mode = "", string newpassword = "");
        void ChangePasswordUpdate(User obj, string mode = "");
        void UserProfileUpdate(User obj, string mode = "");
        void MultiDelete(List<User> obj, int UserId);
        string CheckUserExist(int UserId, string Email, string strDBConn);
    }
}
