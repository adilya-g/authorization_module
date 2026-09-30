using ExampleApplication.Database;
using ExampleApplication.Entity;
using Microsoft.EntityFrameworkCore;

namespace ExampleApplication.Repository;

public class UserRepository
{
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db) => _db = db;

    public async Task AddUserAsync(User user, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        user.CreatedAt ??= now;
        user.UpdatedAt ??= now;

        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
        // после SaveChanges у user.UserId будет сгенерированный БД id
    }

    public async Task<bool> RemoveUserAsync(int userId, CancellationToken ct = default)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);
        if (user is null) return false;

        _db.Users.Remove(user);
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task UpdateUserAsync(User user, CancellationToken ct = default)
    {
        user.UpdatedAt = DateTime.UtcNow;
        _db.Users.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public Task<User?> FindUserByIdAsync(int userId, CancellationToken ct = default)
        => _db.Users.FirstOrDefaultAsync(u => u.UserId == userId, ct);

    public Task<User?> FindUserByEmailAsync(string email, CancellationToken ct = default)
        => _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<List<User>> GetUsersByCreatedPeriodAsync(DateTime start, DateTime end, CancellationToken ct = default)
        => _db.Users
              .Where(u => u.CreatedAt >= start && u.CreatedAt <= end)
              .AsNoTracking()
              .ToListAsync(ct);

    public Task<List<User>> GetUsersByUpdatedPeriodAsync(DateTime start, DateTime end, CancellationToken ct = default)
        => _db.Users
              .Where(u => u.UpdatedAt >= start && u.UpdatedAt <= end)   // было CreatedAt — это баг
              .AsNoTracking()
              .ToListAsync(ct);
}