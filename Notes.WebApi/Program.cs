using System.Reflection;
using AutoMapper;
using Notes.Application;
using Notes.Application.Common.Mappings;
using Notes.Application.Interfaces;
using Notes.Persistance;

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

app.UseExceptionHandler();
app.UseRouting();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();