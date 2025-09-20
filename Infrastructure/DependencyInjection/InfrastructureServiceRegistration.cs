using Infrastructure.Persistence.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using APICoursePlatform.Helpers;
using APICoursePlatform.Models;
using APICoursePlatform.Repository;
using APICoursePlatform.RepositoryContract;
using APICoursePlatform.UnitOfWorkContract;

namespace APICoursePlatform.Infrastructure
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // EF Core
            services.AddDbContext<CoursePlatformContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("CourseString")));

            // Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<INotificationRepository, NotificationRepository>();

            // Services
            services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<PayPalService>();

            return services;
        }
    }
}
