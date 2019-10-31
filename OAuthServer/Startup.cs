using System;
using System.Collections.Generic;
using System.Text;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;
using Owin;

namespace OAuthServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(Clients.Get())
                    .UseInMemoryScopes(Scopes.Get())
                    .UseInMemoryUsers(new List<InMemoryUser>()),

                RequireSsl = false
            };

            app.UseIdentityServer(options);
        }
    }
}
