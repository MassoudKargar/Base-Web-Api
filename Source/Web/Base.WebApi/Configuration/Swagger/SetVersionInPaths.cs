
namespace Ccms.WebFramework.Swagger
{
    using Microsoft.OpenApi.Models;

    using Swashbuckle.AspNetCore.SwaggerGen;
    
    public class SetVersionInPaths : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var updatedPaths = new OpenApiPaths();

            foreach ((string key, OpenApiPathItem value) in swaggerDoc.Paths)
            {
                updatedPaths.Add(
                    key.Replace("v{version}", swaggerDoc.Info.Version),
                    value);
            }
            swaggerDoc.Paths = updatedPaths;
        }
    }
}
