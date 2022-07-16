using hospins.Repository.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace hospins.Infrastructure
{
    public static class ConfigurationSettings
    {
        public static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        public static string DBConnection
        {
            get
            {
                IOptions<ConnectionStrings> _connectionstrings = (IOptions<ConnectionStrings>)HttpContext.RequestServices.GetService(typeof(IOptions<ConnectionStrings>));
                return _connectionstrings.Value.StrDbConn;
            }
        }
    }
}
