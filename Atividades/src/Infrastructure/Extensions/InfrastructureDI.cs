using CutoverManager.Domain.Interfaces;
using CutoverManager.Domain.Services;
using CutoverManager.Infrastructure.Context;
using CutoverManager.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CutoverManager.Infrastructure.Extensions;

public static class InfrastructureDI
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlite(connectionString));

        // Repositórios
        services.AddScoped<IPlanoRepository, PlanoRepository>();
        services.AddScoped<IAreaRepository, AreaRepository>();
        services.AddScoped<IExecutorRepository, ExecutorRepository>();
        services.AddScoped<ISistemaRepository, SistemaRepository>();
        services.AddScoped<IMarcoRepository, MarcoRepository>();
        services.AddScoped<IAtividadeRepository, AtividadeRepository>();


        // Domain Services (REGRAS DE NEGÓCIO)
        services.AddScoped<PlanoDomainService>();
        services.AddScoped<AtividadeDomainService>();

        return services;
    }
}