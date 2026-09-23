using Microsoft.AspNetCore.Identity;
using ExampleApplication.Entity;

namespace ExampleApplication.Service;

public class AuthorizationService(UserManageService _userManageService)
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    
    public void CreateUser(User user, string password)
    {
        user.HashedPassword = _passwordHasher.HashPassword(user, password);
        _userManageService.AddUser(user);
    }

    public bool VerifyPassword(string password, User user)
    {
        return _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password) == PasswordVerificationResult.Success;
    }
}
