using Microsoft.Extensions.DependencyInjection;
using UsersApp.Application.Users;

namespace UsersApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        return services;
    }
}