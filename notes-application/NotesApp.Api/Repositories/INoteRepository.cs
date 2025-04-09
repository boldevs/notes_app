using NotesApp.Api.Dtos;
using NotesApp.Api.Models;

namespace NotesApp.Api.Repositories
{
    public interface INoteRepository
    {
        Task<IEnumerable<Note>> GetAllByUserAsync(int userId, NoteQueryParams query);
        Task<int> CountByUserAsync(int userId, NoteQueryParams query);
        Task<Note?> GetByIdAsync(int id, int userId);
        Task<int> CreateAsync(Note note);
        Task<bool> UpdateAsync(Note note);
        Task<bool> DeleteAsync(int id, int userId);
    }

}