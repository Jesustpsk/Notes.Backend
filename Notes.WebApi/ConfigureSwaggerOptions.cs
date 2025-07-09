using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Notes.WebApi;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
    {
        _provider = provider;
    }

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var apiVersion = description.ApiVersion.ToString();
            options.SwaggerDoc(description.GroupName,
                new OpenApiInfo
                {
                    Version = apiVersion,
                    Title = $"Notes API {description.ApiVersion}",
                    Description = "A simple example ASP.NET Core Web API",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Notes Chat",
                        Email = string.Empty,
                        Url = new Uri("https://t.me/Jesustpsk")
                    }, License = new OpenApiLicense
                    {
                        Name = "Notes Telegram Chat",
                        Url = new Uri("https://t.me/chat")
                    }
                });
            
            options.AddSecurityDefinition($"AuthToken {apiVersion}",
                new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Name = "Authorization",
                    Description = "JWT Authorization header using the Bearer scheme."
                });
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = $"AuthToken {apiVersion}"
                        }
                    },
                    []
                }
            });
            
            options.CustomOperationIds(apiDescription => 
                apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                ? methodInfo.Name
                : null);
        }
    }
}