using System.Reflection;
using BandLookMVC.Repositories;

namespace BandLookMVC;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("DBConnection") ??
            throw new ArgumentNullException(nameof(configuration));
        

        var applicationAssembly = typeof(DependencyInjection).GetTypeInfo().Assembly;

        services.AddSingleton(new MySqlConnectionFactory(connectionString))
            .AddRepositories();
        
        return services;
    }
    
    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        // Register repositories using reflection based on naming convention
        var repositoryTypes = Assembly
            .GetAssembly(
                typeof(ArtistRepository)) // Assuming SubjectRepository is representative of the assembly containing all repositories
            ?.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Repository"))
            .ToList();

        foreach (var type in repositoryTypes)
        {
            var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == "I" + type.Name);
            if (interfaceType != null)
            {
                services.AddScoped(interfaceType, type);
            }
        }

        return services;
    }

    // private static IServiceCollection AddServices(this IServiceCollection services)
    // {
    //     // Register services using reflection based on naming convention
    //     var serviceTypes = Assembly
    //         .GetAssembly(
    //             typeof(TokenService)) // Assuming TokenService is representative of the assembly containing all services
    //         ?.GetTypes()
    //         .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
    //         .ToList();
    //
    //     foreach (var type in serviceTypes)
    //     {
    //         var interfaceType = type.GetInterfaces().FirstOrDefault(i => i.Name == "I" + type.Name);
    //         if (interfaceType != null)
    //         {
    //             services.AddScoped(interfaceType, type);
    //         }
    //     }
    //
    //     return services;
    // }
}