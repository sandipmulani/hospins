using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using hospins.Repository.Models;
using System.Collections.Generic;

namespace hospins.Infrastructure
{
    public static class CurrentContext
    {
        public static HttpContext HttpContext => new HttpContextAccessor().HttpContext;

        public static Repository.Data.User UserDetail
        {
            get
            {
                if (HttpContext?.Session != null)
                {
                    string userSession = HttpContext.Session.TryGetValue("UserDetail", out byte[] outValue).ToString();

                    if (userSession == "True")
                    {
                        userSession = Encoding.ASCII.GetString(outValue, 0, outValue.Length);
                        return JsonConvert.DeserializeObject<hospins.Repository.Data.User>(userSession);
                    }
                    else
                    {
                        return null;
                    }
                }
                return null;
            }
            set
            {
                if (HttpContext?.Session != null)
                {
                    byte[] userValue = Encoding.ASCII.GetBytes(JsonConvert.SerializeObject(value, Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        PreserveReferencesHandling = PreserveReferencesHandling.Objects
                    }));
                    HttpContext.Session.Set("UserDetail", userValue);
                }
            }
        }
    }
}