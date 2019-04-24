using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.AzureAD.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using AADWepApi.Filters;

namespace AADWepApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(AzureADDefaults.BearerAuthenticationScheme)
                .AddAzureADBearer(options => Configuration.Bind("AzureAd", options));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Protected Api",
                });

                c.OperationFilter<SecurityRequirementsOperationFilter>();


                string tenantId = Configuration.GetValue<string>("AzureAd:TenantId");
                //IMATE - StevensW
                // Define the OAuth2.0 scheme that's in use (i.e. Implicit Flow)
                c.AddSecurityDefinition("oauth2", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "implicit",
                    AuthorizationUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize",
                    TokenUrl = $"https://login.microsoftonline.com/common/{tenantId}/v2.0/token",
                    Scopes = new Dictionary<string, string>
                   {
                       { "openid", "Unsure" },
                       { "profile", "Also Unsure" }
                   }
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.OAuthClientId(Configuration.GetValue<string>("AzureAd:ClientId"));
                c.OAuthAppName("Protected API");

                //c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
                //c.OAuthClientSecret(Configuration.GetValue<string>("AzureAd:ClientId"));
            });

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
