using CutoverManager.Application.Interfaces;
using CutoverManager.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CutoverManager.Application.Extensions;

public static class ApplicationDI
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Services
        services.AddScoped<IPlanoService, PlanoService>();
        services.AddScoped<IAtividadeService, AtividadeService>();
        services.AddScoped<IAreaService, AreaService>();
        services.AddScoped<IExecutorService, ExecutorService>();
        services.AddScoped<ISistemaService, SistemaService>();
        services.AddScoped<IMarcoService, MarcoService>();

        return services;
    }
}