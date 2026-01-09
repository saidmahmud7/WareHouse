using Infrastructure.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Extensions;
using Serilog;
using Swashbuckle.AspNetCore.Swagger;

var builder = WebApplication.CreateBuilder(args);
builder.Host.AddSerilogLogger();
builder.Host.UseSerilog();
builder.Services.AddMemoryCache();

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddOpenApi();
builder.Services.SwaggerConfigurationServices();


builder.Services.AddServices(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("*")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var swaggerProvider = scope.ServiceProvider.GetRequiredService<ISwaggerProvider>();
    var swagger = swaggerProvider.GetSwagger("v1");

    var swaggerJson = swagger.SerializeAsJson(Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
    File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "swagger.json"), swaggerJson);
}

app.UseRouting();
app.UseCors("AllowReactApp");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapGet("/", () => "API работает!");
app.Run();
