using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistance;
using Notes.WebApi;
using Notes.WebApi.Middleware;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

var mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
    cfg.AddProfile(new AssemblyMappingProfile(typeof(INotesDbContext).Assembly));
});

var mapper = mapperConfig.CreateMapper();
services.AddSingleton(mapper);

services.AddApplication();
services.AddPersistance(configuration);
services.AddControllers();

services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowAnyOrigin();
    });
});

services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:44367/";
        options.Audience = "NotesWebApi";
        options.RequireHttpsMetadata = false;
    });

services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
services.AddSwaggerGen(config =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    config.IncludeXmlComments(xmlPath);
});

services.AddApiVersioning();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<NotesDbContext>();
    await DbInitializer.InitializeAsync(context);
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

app.UseSwagger();
app.UseSwaggerUI(config =>
{
    foreach (var description in provider.ApiVersionDescriptions)
    {
        config.SwaggerEndpoint(
            $"/swagger/{description.GroupName}/swagger.json",
            description.GroupName.ToUpperInvariant());
        config.RoutePrefix = string.Empty;    
    }
});
app.UseCustomExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseApiVersioning();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();