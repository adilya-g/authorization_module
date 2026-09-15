using ExampleApplication.Entity;
using ExampleApplication.Repository;

namespace ExampleApplication.Service;

public class UserManageService(UserRepository userRepository)
{
    public bool Update(User user)
    {
        if (user.UserId == null)
        {
            var foundUser = userRepository.FindUserByEmail(user.Email);
            if (foundUser == null)
                return false;
            user.UserId = foundUser.UserId;
        }
        userRepository.UpdateUser(user);
        return true;
    }

    public bool Delete(int userId)
    {
        var user = FindUserById(userId);
        return user != null && userRepository.RemoveUser(user);
    }

    public User? FindUserById(int userId)
    {
        return userRepository.FindUserById(userId);
    }
    
    public User? FindUserByEmail(string email)
    {
        return userRepository.FindUserByEmail(email);
    }

    public void AddUser(User user)
    {
        userRepository.AddUser(user);
    }
}