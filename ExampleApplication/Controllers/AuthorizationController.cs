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
    public async Task<IActionResult> GetUsersByUpdatedPeriod([FromQuery][Required] DateTime startDate,[FromQuery][Required] DateTime endDate)
    {
        if (!ValidationHelper.TryValidatePeriod(startDate, endDate, out var error))
        {
            return new BadRequestObjectResult(error);
        }
        var midResult = await userManageService.GetUsersByUpdatedPeriod(startDate, endDate);
        var result = midResult.Select(u => u.MapUserToUserDto()).ToList();
        return new JsonResult(result);
    }
    
    [HttpGet("users/created/period")]
    public async Task<IActionResult> GetUsersByCreatedPeriod([FromQuery][Required] DateTime startDate,[FromQuery][Required] DateTime endDate)
    {
        if (!ValidationHelper.TryValidatePeriod(startDate, endDate, out var error))
        {
            return new BadRequestObjectResult(error);
        }
        var midResult = await userManageService.GetUsersByCreatedPeriod(startDate, endDate);
        var result = midResult.Select(u => u.MapUserToUserDto()).ToList();
        return new JsonResult(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var user = await userManageService.FindUserByEmail(loginDto.email);
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
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        if (await userManageService.Delete(userId))
            return Ok();
        return NotFound();
    }
}