using System;
using ComicsApi.Application.Interfaces;
using ComicsApi.Domain.Interfaces;
using ComicsApi.Infrastructure.Data;
using ComicsApi.Infrastructure.Repositories;
using ComicsApi.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace ComicsApi.Infrastructure
{
    /// <summary>
    /// Lớp đăng ký các dịch vụ của tầng Infrastructure
    /// </summary>
    public static class DependencyInjection
    {
        /// <summary>
        /// Đăng ký các dịch vụ của tầng Infrastructure
        /// </summary>
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Đăng ký DbContext với cấu hình JSON support
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(configuration.GetConnectionString("DefaultConnection"));
            dataSourceBuilder.EnableDynamicJson();
            var dataSource = dataSourceBuilder.Build();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(dataSource,
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            // Đăng ký các repository
            services.AddScoped<IMangaRepository, MangaRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<IChapterImageRepository, ChapterImageRepository>();
            services.AddScoped<ICrawlLogRepository, CrawlLogRepository>();

            // Đăng ký các service
            services.AddHttpClient();
            services.AddScoped<ICrawlerService, CrawlerService>();

            return services;
        }
    }
}