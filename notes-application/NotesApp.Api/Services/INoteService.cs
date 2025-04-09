using NotesApp.Api.Dtos;
using NotesApp.Api.Models;

namespace NotesApp.Api.Services
{
    public interface INoteService
    {
        Task<PagedResult<NoteResponseDto>> GetAllAsync(int userId, NoteQueryParams query);
        Task<NoteResponseDto?> GetByIdAsync(int id, int userId);
        Task<int> CreateAsync(CreateNoteDto dto, int userId);
        Task<bool> UpdateAsync(int id, UpdateNoteDto dto, int userId);
        Task<bool> DeleteAsync(int id, int userId);
    }
}