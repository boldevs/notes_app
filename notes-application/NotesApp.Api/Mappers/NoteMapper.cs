using NotesApp.Api.Dtos;
using NotesApp.Api.Models;

namespace NotesApp.Api.Mappers
{
    public static class NoteMapper
    {
        public static NoteResponseDto ToDto(Note note)
        {
            return new NoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }
    }
}