using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace hospins.UserAgents
{
    public class UserAgent
    {
        private readonly string _userAgent;

        private ClientBrowser _browser;

        public ClientBrowser Browser
        {
            get
            {
                return _browser ?? (_browser = new ClientBrowser(_userAgent));
            }
        }

        private ClientOS _os;

        public ClientOS OS
        {
            get
            {
                return _os ?? (_os = new ClientOS(_userAgent));
            }
        }

        public UserAgent(string userAgent)
        {
            _userAgent = userAgent;
        }
    }
}
