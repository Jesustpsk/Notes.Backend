using System.Reflection;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Notes.Application.Notes.Queries.GetNote;

namespace Notes.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options => options.RegisterServicesFromAssembly(typeof(GetNoteQueryHandler).Assembly));
        return services;
    }
}