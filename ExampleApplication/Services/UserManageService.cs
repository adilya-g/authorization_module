using ExampleApplication.Entity;
using ExampleApplication.Repository;

namespace ExampleApplication.Service;

public class UserManageService(UserRepository userRepository)
{
    public async Task<bool> Update(User user)
    {
        if (user.UserId == null)
        {
            var foundUser = await userRepository.FindUserByEmailAsync(user.Email);
            if (foundUser == null)
                return false;
            user.UserId = foundUser.UserId;
        }
        user.UpdatedAt = DateTime.Now;
        await userRepository.UpdateUserAsync(user);
        return true;
    }

    public async Task<bool> Delete(int userId)
    {
        return await userRepository.RemoveUserAsync(userId);
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
        userRepository.AddUserAsync(user);
    }

    public async Task<List<User>?> GetUsersByCreatedPeriod(DateTime startDate, DateTime endDate)
    {
        return await userRepository.GetUsersByCreatedPeriodAsync(startDate, endDate);
    }

    public async Task<List<User>?> GetUsersByUpdatedPeriod(DateTime startDate, DateTime endDate)
    {
        return await userRepository.GetUsersByUpdatedPeriodAsync(startDate, endDate);
    }
}