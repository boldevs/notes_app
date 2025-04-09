using System.Data;
using Microsoft.Data.SqlClient;
using NotesApp.Api.Auth;
using NotesApp.Api.Repositories;
using NotesApp.Api.Services;

namespace NotesApp.Api.Extensions
{
    public static class ServiceExtensions
    {
        public static void RegisterAppServices(this IServiceCollection services, IConfiguration config)
        {
            // JWT settings
            services.Configure<JwtSettings>(config.GetSection("Jwt"));

            // Custom Services
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<INoteRepository, NoteRepository>();
            services.AddScoped<INoteService, NoteService>();
            services.AddScoped<IAuthService, AuthService>();

            // DB connection
            services.AddScoped<IDbConnection>(sp =>
                new SqlConnection(config.GetConnectionString("DefaultConnection")));
        }
    }
}