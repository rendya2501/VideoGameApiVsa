using Carter;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using VideoGameApiVsa.Behaviors;
using VideoGameApiVsa.Data;

namespace VideoGameApiVsa.Extensions;

/// <summary>
/// アプリケーションサービスの登録
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// データベース関連サービスの登録
    /// </summary>
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VideoGameDbContext>(options =>
            options.UseInMemoryDatabase("GameDB"));

        return services;
    }

    /// <summary>
    /// アプリケーション層のサービス登録（MediatR、Carter）
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // MediatR（CQRS）
        services.AddMediatR(cfg =>
            cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

        // Carter（Minimal API拡張）
        services.AddCarter();

        return services;
    }

    /// <summary>
    /// バリデーション関連サービスの登録
    /// </summary>
    public static IServiceCollection AddValidationServices(this IServiceCollection services)
    {
        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(Program).Assembly);

        // MediatR Pipeline Behaviors（実行順序: 登録順）
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
