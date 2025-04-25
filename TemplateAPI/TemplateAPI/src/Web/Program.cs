using Serilog;
using TemplateAPI.Infrastructure;
using TemplateAPI.Infrastructure.Persistence;
using TemplateAPI.Web;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.AddKeyVaultIfConfigured();
builder.AddApplicationServices();
builder.AddInfrastructureServices();
builder.AddWebServices();
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));
builder.Services.AddSwaggerDocument();

WebApplication app = builder.Build();
// Configure the HTTP request pipeline.
await app.InitialiseDatabaseAsync();
app.UseHsts();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUi();
}

// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseHealthChecks("/health");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseSerilogRequestLogging();
app.UseSwaggerUi(settings =>
{
    settings.Path = "/api";
    settings.DocumentPath = "/api/specification.json";
});
app.UseExceptionHandler(options => { });
app.MapControllers();
app.MapDefaultControllerRoute();
app.Run();

public partial class Program
{
}
