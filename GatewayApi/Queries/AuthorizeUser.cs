using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GatewayApi.Queries
{
    public class AuthorizeUser
    {
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
