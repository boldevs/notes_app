using System.Data;
using Dapper;
using NotesApp.Api.Dtos;
using NotesApp.Api.Models;

namespace NotesApp.Api.Repositories
{
    public class NoteRepository : INoteRepository
    {
        private readonly IDbConnection _db;

        public NoteRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<int> CountByUserAsync(int userId, NoteQueryParams query)
        {
            var sql = @"SELECT COUNT(*) FROM Notes
                WHERE UserId = @UserId";

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                sql += " AND Title LIKE @Search";
            }

            return await _db.ExecuteScalarAsync<int>(sql, new
            {
                UserId = userId,
                Search = $"%{query.Search}%"
            });
        }

        public async Task<int> CreateAsync(Note note)
        {
            var sql = @"INSERT INTO Notes (Title, Content, CreatedAt, UpdatedAt, UserId)
                    VALUES (@Title, @Content, @CreatedAt, @UpdatedAt, @UserId);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await _db.ExecuteScalarAsync<int>(sql, note);
        }

        public async Task<bool> DeleteAsync(int id, int userId)
        {
            var sql = "DELETE FROM Notes WHERE Id = @Id AND UserId = @UserId";
            var affectedRows = await _db.ExecuteAsync(sql, new { Id = id, UserId = userId });
            return affectedRows > 0;
        }
        public async Task<IEnumerable<Note>> GetAllByUserAsync(int userId, NoteQueryParams query)
        {
            var offset = (query.Page - 1) * query.Limit;
            var sql = $@"
                            SELECT * FROM Notes
                            WHERE UserId = @UserId
                            {(string.IsNullOrWhiteSpace(query.Search) ? "" : "AND Title LIKE @Search")}
                            ORDER BY 
                                {(query.SortBy == "title" ? "Title" : "CreatedAt")} {(query.SortDesc ? "DESC" : "ASC")}
                            OFFSET @Offset ROWS FETCH NEXT @Limit ROWS ONLY;
                        ";

            return await _db.QueryAsync<Note>(sql, new
            {
                UserId = userId,
                Search = $"%{query.Search}%",
                Offset = offset,
                Limit = query.Limit
            });
        }

        public async Task<Note?> GetByIdAsync(int id, int userId)
        {
            var sql = "SELECT * FROM Notes WHERE Id = @Id AND UserId = @UserId";
            return await _db.QueryFirstOrDefaultAsync<Note>(sql, new { Id = id, UserId = userId });
        }

        public async Task<bool> UpdateAsync(Note note)
        {
            var sql = @"UPDATE Notes
                    SET Title = @Title,
                        Content = @Content,
                        UpdatedAt = @UpdatedAt
                    WHERE Id = @Id AND UserId = @UserId";

            var affectedRows = await _db.ExecuteAsync(sql, note);
            return affectedRows > 0;
        }
    }
}