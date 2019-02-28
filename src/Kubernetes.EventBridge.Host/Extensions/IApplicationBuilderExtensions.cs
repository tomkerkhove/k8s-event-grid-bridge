using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Kubernetes.EventBridge.Host.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        /// <summary>
        ///     Add support for Open API with API explorer
        /// </summary>
        /// <param name="app">Application Builder</param>
        public static void UseOpenApiUi(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(swaggerUiOptions =>
            {
                swaggerUiOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Kubernetes Event Bridge API");
                swaggerUiOptions.DisplayOperationId();
                swaggerUiOptions.EnableDeepLinking();
                swaggerUiOptions.DocumentTitle = "Kubernetes Event Bridge API";
                swaggerUiOptions.DocExpansion(DocExpansion.List);
                swaggerUiOptions.DisplayRequestDuration();
                swaggerUiOptions.EnableFilter();
            });
        }
    }
}
