using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.Linq;

namespace AADWepApi.Filters
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(Swashbuckle.AspNetCore.Swagger.Operation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            var requiredScopes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(attr => attr.Policy)
                .Distinct();

            if (requiredScopes.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });

                if (requiredScopes.FirstOrDefault() == null)
                {
                    requiredScopes = (new List<string>() { "openid" });
                }

                operation.Security = new List<IDictionary<string, IEnumerable<string>>>();
                operation.Security.Add(new Dictionary<string, IEnumerable<string>>
            {
                { "oauth2", requiredScopes }
            });
            }
        }
    }
}
