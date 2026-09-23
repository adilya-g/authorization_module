using ExampleApplication.Entity;

namespace ExampleApplication.DTO;

public static class DtoMapping
{
    public static User MapRegisterDtoToUser(this RegisterDto registerDto)
    {
        var user = new User();
        user.Email = registerDto.Email;
        user.FirstName = registerDto.FirstName;
        user.LastName = registerDto.LastName;
        user.HashedPassword = "";
        user.CreatedAt = DateTime.Now;
        return user;
    }

    public static User MapUserDtoToUser(this UserDto userDto)
    {
        var user = new User();
        user.Email = userDto.Email;
        user.FirstName = userDto.FirstName;
        user.LastName = userDto.LastName;
        user.HashedPassword = "";
        return user;
    }

    public static UserDto? MapUserToUserDto(this User user)
    {
        var userDto = new UserDto();
        userDto.Email = user.Email;
        userDto.FirstName = user.FirstName;
        userDto.LastName = user.LastName;
        userDto.UserId = user.UserId;
        return userDto;
    }
}