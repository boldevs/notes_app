using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using NotesApp.Api.Dtos;
using NotesApp.Api.Services;

namespace NotesApp.Api.Endpoints
{
    public static class NoteEndpoints
    {
        public static void MapNoteEndpoints(this WebApplication app)
        {
            app.MapGet("/notes", [Authorize] async (
                ClaimsPrincipal user,
                [AsParameters] NoteQueryParams query, 
                INoteService noteService) =>
            {
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var result = await noteService.GetAllAsync(userId, query);
                return Results.Ok(result);
            });

            app.MapGet("/notes/{id}", [Authorize] async (
                int id, ClaimsPrincipal user, INoteService noteService) =>
            {
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var note = await noteService.GetByIdAsync(id, userId);
                return note is not null ? Results.Ok(note) : Results.NotFound();
            });

           app.MapPost("/notes", [Authorize] async (
                CreateNoteDto dto,
                ClaimsPrincipal user,
                INoteService noteService,
                HttpContext httpContext) =>
            {
                if (!httpContext.Request.HasJsonContentType())
                    return Results.BadRequest("Invalid content type.");

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(dto, serviceProvider: null, items: null);
                if (!Validator.TryValidateObject(dto, context, validationResults, true))
                    return Results.BadRequest(validationResults);

                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var noteId = await noteService.CreateAsync(dto, userId);
                return Results.Created($"/notes/{noteId}", new { Id = noteId });
            });

            app.MapPut("/notes/{id}", [Authorize] async (
                int id, UpdateNoteDto dto, ClaimsPrincipal user, INoteService noteService, HttpContext httpContext) =>
            {
                if (!httpContext.Request.HasJsonContentType())
                    return Results.BadRequest("Invalid content type.");

                var validationResults = new List<ValidationResult>();
                var context = new ValidationContext(dto, serviceProvider: null, items: null);
                if (!Validator.TryValidateObject(dto, context, validationResults, true))
                    return Results.BadRequest(validationResults);
                    
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var updated = await noteService.UpdateAsync(id, dto, userId);
                return updated ? Results.NoContent() : Results.NotFound();
            });

            app.MapDelete("/notes/{id}", [Authorize] async (
                int id, ClaimsPrincipal user, INoteService noteService) =>
            {
                var userId = int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)!.Value);
                var deleted = await noteService.DeleteAsync(id, userId);
                return deleted ? Results.NoContent() : Results.NotFound();
            });
        }
    }

}