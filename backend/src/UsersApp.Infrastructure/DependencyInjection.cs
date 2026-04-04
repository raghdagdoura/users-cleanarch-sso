using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UsersApp.Application.Common.Interfaces;
using UsersApp.Infrastructure.Email;
using UsersApp.Infrastructure.Persistence;
using UsersApp.Infrastructure.Repositories;

namespace UsersApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("UsersDb"));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IEmailSender, SendGridEmailSender>();

        return services;
    }
}