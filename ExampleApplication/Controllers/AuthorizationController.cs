using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using ExampleApplication.DTO;
using ExampleApplication.Entity;
using ExampleApplication.Helpers;
using ExampleApplication.Repository;
using ExampleApplication.Service;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.CompilerServices;

namespace ExampleApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController(
        UserManageService userManageService,
        AuthorizationService authorizationService
    ): ControllerBase
{
    [HttpGet("users/updated/period")]
    public IActionResult GetUsersByUpdatedPeriod([FromQuery][Required] DateTime startDate,[FromQuery][Required] DateTime endDate)
    {
        if (!ValidationHelper.TryValidatePeriod(startDate, endDate, out var error))
        {
            return new BadRequestObjectResult(error);
        }
        var result = userManageService.GetUsersByUpdatedPeriod(startDate, endDate);
        result.Select(u => u.MapUserToUserDto()).ToList();
        return new JsonResult(JsonSerializer.Serialize(result));
    }
    
    [HttpGet("users/created/period")]
    public IActionResult GetUsersByCreatedPeriod([FromQuery][Required] DateTime startDate,[FromQuery][Required] DateTime endDate)
    {
        if (!ValidationHelper.TryValidatePeriod(startDate, endDate, out var error))
        {
            return new BadRequestObjectResult(error);
        }
        var result = userManageService.GetUsersByCreatedPeriod(startDate, endDate);
        result.Select(u => u.MapUserToUserDto()).ToList();
        return new JsonResult(JsonSerializer.Serialize(result));
    }
    
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