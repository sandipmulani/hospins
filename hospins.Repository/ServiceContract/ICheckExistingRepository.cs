using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using hospins.Repository.Data;
namespace hospins.Repository.ServiceContract
{
    public interface ICheckExistingRepository : IDisposable
    {
        string CheckChildRecordExistsForDelete(string SpName, IDictionary<string, object> _dic, string strDBConn = "");
    }
}
