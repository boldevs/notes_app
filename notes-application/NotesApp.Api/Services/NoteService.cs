using NotesApp.Api.Dtos;
using NotesApp.Api.Models;
using NotesApp.Api.Repositories;

namespace NotesApp.Api.Services
{
    public class NoteService : INoteService
    {
        private readonly INoteRepository _noteRepo;

        public NoteService(INoteRepository noteRepo)
        {
            _noteRepo = noteRepo;
        }

        public async Task<int> CreateAsync(CreateNoteDto dto, int userId)
        {
            var now = DateTime.UtcNow;

            var note = new Note
            {
                Title = dto.Title,
                Content = dto.Content,
                CreatedAt = now,
                UpdatedAt = now,
                UserId = userId
            };

            return await _noteRepo.CreateAsync(note);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            return await _noteRepo.DeleteAsync(id, userId);
        }
        public async Task<PagedResult<NoteResponseDto>> GetAllAsync(int userId, NoteQueryParams query)
        {
            var totalItems = await _noteRepo.CountByUserAsync(userId, query);

            var notes = await _noteRepo.GetAllByUserAsync(userId, query);

            // 3. Map to DTOs
            var noteDtos = notes.Select(note => new NoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            });

            // 4. Return in paged result
            return new PagedResult<NoteResponseDto>
            {
                Items = noteDtos,
                TotalItems = totalItems,
                Page = query.Page,
                Limit = query.Limit
            };
        }

        public async Task<NoteResponseDto?> GetByIdAsync(int id, int userId)
        {
            var note = await _noteRepo.GetByIdAsync(id, userId);
            if (note == null) return null;

            return new NoteResponseDto
            {
                Id = note.Id,
                Title = note.Title,
                Content = note.Content,
                CreatedAt = note.CreatedAt,
                UpdatedAt = note.UpdatedAt
            };
        }

        public async Task<bool> UpdateAsync(int id, UpdateNoteDto dto, int userId)
        {
            var existing = await _noteRepo.GetByIdAsync(id, userId);
            if (existing == null) return false;

            existing.Title = dto.Title;
            existing.Content = dto.Content;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _noteRepo.UpdateAsync(existing);
        }
    }
}