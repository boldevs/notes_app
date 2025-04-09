using System.Data;
using Dapper;
using NotesApp.Api.Models;

namespace NotesApp.Api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IDbConnection _db;
        public UserRepository(IDbConnection db)
        {
            _db = db;
        }
        public async Task<int> CreateAsync(User user)
        {
            var sql = @"INSERT INTO Users (Username, PasswordHash, CreatedAt)
                    VALUES (@Username, @PasswordHash, @CreatedAt);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            return await _db.ExecuteScalarAsync<int>(sql, user);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var sql = "SELECT * FROM Users WHERE Username = @Username";
            return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Username = username });
        }
    }
}