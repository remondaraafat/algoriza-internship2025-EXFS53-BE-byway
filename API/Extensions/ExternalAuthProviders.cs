using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.DependencyInjection
{
    public static class ExternalAuthProviders
    {
        public static AuthenticationBuilder AddExternalAuthProviders(
    this AuthenticationBuilder builder, IConfiguration config)
        {
            builder.AddGoogle(options =>
            {
                options.ClientId = config["Authentication:Google:ClientId"];
                options.ClientSecret = config["Authentication:Google:ClientSecret"];
            });

            builder.AddFacebook(options =>
            {
                options.AppId = config["Authentication:Facebook:AppId"];
                options.AppSecret = config["Authentication:Facebook:AppSecret"];
            });

            builder.AddMicrosoftAccount(options =>
            {
                options.ClientId = config["Authentication:Microsoft:ClientId"];
                options.ClientSecret = config["Authentication:Microsoft:ClientSecret"];
            });

            return builder;
        }

    }
}
