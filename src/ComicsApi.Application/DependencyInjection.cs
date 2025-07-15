using System.Reflection;
using ComicsApi.Application.Mappings;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ComicsApi.Application
{
    /// <summary>
    /// Lớp đăng ký các dịch vụ của tầng Application
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Đăng ký các dịch vụ của tầng Application
        /// </summary>
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Đăng ký MediatR
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Đăng ký AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}