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
}