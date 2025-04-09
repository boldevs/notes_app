using NotesApp.Api.Dtos;
using NotesApp.Api.Services;

namespace NotesApp.Api.Endpoints
{
    public static class AuthEndpoints
    {
        public static void MapAuthEndpoints(this WebApplication app)
        {
            app.MapPost("/auth/register", async (
                RegisterDto dto, IAuthService authService) =>
            {
                var result = await authService.RegisterAsync(dto);
                return result.Success ? Results.Ok(result) : Results.BadRequest(result.Error);
            });

            app.MapPost("/auth/login", async (
                LoginDto dto, IAuthService authService) =>
            {
                var result = await authService.LoginAsync(dto);
                return result.Success ? Results.Ok(result) : Results.Unauthorized();
            });
        }
    }

}