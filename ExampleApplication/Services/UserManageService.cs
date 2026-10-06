using Microsoft.Extensions.Logging;
using ExampleApplication.Entity;
using ExampleApplication.Repository;

namespace ExampleApplication.Service;

public class UserManageService(
    UserRepository userRepository,
    ILogger<UserManageService> _logger) // Добавлен логгер
{
    public async Task<bool> Update(User user)
    {
        if (user.UserId == null)
        {
            var foundUser = await userRepository.FindUserByEmailAsync(user.Email);
            if (foundUser == null)
            {
                _logger.LogWarning("Попытка обновления несуществующего пользователя по Email: {Email}", user.Email);
                return false;
            }
            user.UserId = foundUser.UserId;
        }
        
        user.UpdatedAt = DateTime.UtcNow;
        await userRepository.UpdateUserAsync(user);
        _logger.LogInformation("Пользователь {UserId} успешно обновлен", user.UserId);
        return true;
    }

    public async Task<bool> Delete(int userId)
    {
        var result = await userRepository.RemoveUserAsync(userId);
        if (result)
        {
            _logger.LogInformation("Пользователь {UserId} успешно удален", userId);
        }
        else
        {
            _logger.LogWarning("Попытка удаления несуществующего пользователя с ID: {UserId}", userId);
        }
        return result;
    }

    public async Task<User?> FindUserById(int userId)
    {
        return await userRepository.FindUserByIdAsync(userId);
    }
    
    public async Task<User?> FindUserByEmail(string email)
    {
        return await userRepository.FindUserByEmailAsync(email);
    }

    public async Task AddUser(User user)
    {
        _logger.LogInformation("Добавление пользователя в БД: {Email}", user.Email);
        await userRepository.AddUserAsync(user);
    }

    public async Task<List<User>?> GetUsersByCreatedPeriod(DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Запрос пользователей за период создания: {StartDate} - {EndDate}", startDate, endDate);
        return await userRepository.GetUsersByCreatedPeriodAsync(startDate, endDate);
    }

    public async Task<List<User>?> GetUsersByUpdatedPeriod(DateTime startDate, DateTime endDate)
    {
        _logger.LogInformation("Запрос пользователей за период обновления: {StartDate} - {EndDate}", startDate, endDate);
        return await userRepository.GetUsersByUpdatedPeriodAsync(startDate, endDate);
    }
}