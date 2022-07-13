using Sejil;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog();
// Configure Sejil
builder.Host.UseSejil("/AdminLog",
    minLogLevel: LogLevel.Information,
    writeToProviders: true);
builder.Services.ConfigureSejil(cfg => cfg.Title = "My App Logs");
//////////////////

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
// Add sejil to request pipeline
app.UseSejil();
//////////////////

app.Run();