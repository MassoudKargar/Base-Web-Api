var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory(builder => builder.AddServices()));
builder.Services.RegisterWebApiServices(builder.Configuration);
var app = builder.Build();
app.UseCustomExceptionHandler();
app.UseCors(o =>
{
    o.AllowAnyHeader();
    o.AllowAnyMethod();
    o.AllowAnyOrigin();
});
app.UseHttpsRedirection();
app.UseSwaggerAndUi();
app.UseStaticFiles();
app.UseDefaultFiles();
app.UseSerilogRequestLogging();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.Run();