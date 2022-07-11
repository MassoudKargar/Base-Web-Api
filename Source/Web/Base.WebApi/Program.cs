var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterApplicationServices(builder.Configuration);
builder.Services.RegisterInfrastructerServices(builder.Configuration);
builder.Services.RegisterWebApiServices(builder.Configuration);
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();