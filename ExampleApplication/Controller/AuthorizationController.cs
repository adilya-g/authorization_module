using ExampleApplication.DTO;
using ExampleApplication.Entity;
using ExampleApplication.Repository;
using ExampleApplication.Service;
using Microsoft.AspNetCore.Mvc;

namespace ExampleApplication.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController(
        UserManageService userManageService,
        AuthorizationService authorizationService
    ): ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        var user = userManageService.FindUserByEmail(loginDto.email);
        if (user == null)
            return NotFound();
        var passed = authorizationService.VerifyPassword(loginDto.email, user);
        if (passed)
        {
            return Ok();
        }
        return new UnauthorizedResult();
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto registerDto)
    {
        var rawPassword = registerDto.RawPassword;
        var user = registerDto.MapRegisterDtoToUser();
        authorizationService.CreateUser(user, rawPassword);
        return Ok();
    }

    [HttpPut("user")]
    public IActionResult EditUser([FromBody] UserDto userDto)
    {
        var user = userDto.MapUserDtoToUser();
        userManageService.Update(user);
        return Ok();
    }

    [HttpDelete("user/{userId}")]
    public IActionResult DeleteUser([FromRoute] int userId)
    {
        if (userManageService.Delete(userId))
            return Ok();
        return NotFound();
    }
}