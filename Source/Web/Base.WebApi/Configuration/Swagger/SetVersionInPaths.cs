namespace Base.WebApi.Configuration.Swagger;

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