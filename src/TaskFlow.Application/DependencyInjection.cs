using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.UseCases.Auth;
using TaskFlow.Application.UseCases.Tarefas;

namespace TaskFlow.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<RegistrarUsuarioUseCase>();
        services.AddScoped<LoginUseCase>();

        services.AddScoped<ListarTarefasUseCase>();
        services.AddScoped<ObterTarefaUseCase>();
        services.AddScoped<CriarTarefaUseCase>();
        services.AddScoped<AtualizarTarefaUseCase>();
        services.AddScoped<ConcluirTarefaUseCase>();
        services.AddScoped<DeletarTarefaUseCase>();

        return services;
    }
}
