using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ExampleApplication.Entity;

namespace ExampleApplication.Service;

public class AuthorizationService(
    UserManageService _userManageService,
    ILogger<AuthorizationService> _logger) // Добавлен логгер
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    
    public void CreateUser(User user, string password)
    {
        _logger.LogInformation("Создание нового пользователя: {Email}", user.Email);
        user.HashedPassword = _passwordHasher.HashPassword(user, password);
        _userManageService.AddUser(user);
        _logger.LogInformation("Пользователь {Email} успешно создан и сохранен", user.Email);
    }

    public bool VerifyPassword(string password, User user)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password);
        
        if (result != PasswordVerificationResult.Success)
        {
            _logger.LogWarning("Неверный пароль при попытке входа для пользователя: {Email}", user.Email);
            return false;
        }
        
        _logger.LogInformation("Успешная проверка пароля для пользователя: {Email}", user.Email);
        return true;
    }
}