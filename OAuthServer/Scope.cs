using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer3.Core.Models;

namespace OAuthServer
{
    public static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope> {new Scope {Name = "api1"}};
        }
    }
}
