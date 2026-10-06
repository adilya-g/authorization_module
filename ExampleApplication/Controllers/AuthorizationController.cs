using System.ComponentModel.DataAnnotations;
using ExampleApplication.DTO;
using ExampleApplication.Entity;
using ExampleApplication.Helpers;
using ExampleApplication.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ExampleApplication.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorizationController(
    UserManageService userManageService,
    AuthorizationService authorizationService,
    ILogger<AuthorizationController> _logger // Добавлен логгер
) : ControllerBase
{
    [HttpGet("users/updated/period")]
    public async Task<IActionResult> GetUsersByUpdatedPeriod([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        _logger.LogInformation("Запрос API: получение пользователей по периоду обновления ({StartDate} - {EndDate})", startDate, endDate);
        
        if (!ValidationHelper.TryValidatePeriod(startDate, endDate, out var error))
        {
            _logger.LogWarning("Невалидный период запроса: {Error}", error);
            return new BadRequestObjectResult(error);
        }
        
        var midResult = await userManageService.GetUsersByUpdatedPeriod(startDate, endDate);
        var result = midResult.Select(u => u.MapUserToUserDto()).ToList();
        return new JsonResult(result);
    }
    
    [HttpGet("users/created/period")]
    public async Task<IActionResult> GetUsersByCreatedPeriod([FromQuery][Required] DateTime startDate, [FromQuery][Required] DateTime endDate)
    {
        _logger.LogInformation("Запрос API: получение пользователей по периоду создания ({StartDate} - {EndDate})", startDate, endDate);

        if (!ValidationHelper.TryValidatePeriod(startDate, endDate, out var error))
        {
            _logger.LogWarning("Невалидный период запроса: {Error}", error);
            return new BadRequestObjectResult(error);
        }
        
        var midResult = await userManageService.GetUsersByCreatedPeriod(startDate, endDate);
        var result = midResult.Select(u => u.MapUserToUserDto()).ToList();
        return new JsonResult(result);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        _logger.LogInformation("Попытка входа в систему для Email: {Email}", loginDto.email);

        var user = await userManageService.FindUserByEmail(loginDto.email);
        if (user == null)
        {
            _logger.LogWarning("Вход не удался: пользователь {Email} не найден", loginDto.email);
            return NotFound(new { message = "Пользователь не найден" });
        }

        var passed = authorizationService.VerifyPassword(loginDto.password, user); 
        if (passed)
        {
            _logger.LogInformation("Успешный вход пользователя: {Email} (ID: {UserId})", user.Email, user.UserId);
            return Ok(new { message = "Успешный вход", user.UserId });
        }
        
        return new UnauthorizedResult();
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterDto registerDto)
    {
        _logger.LogInformation("Попытка регистрации нового пользователя: {Email}", registerDto.Email);

        try
        {
            var rawPassword = registerDto.RawPassword;
            var user = registerDto.MapRegisterDtoToUser();
            authorizationService.CreateUser(user, rawPassword);
            
            _logger.LogInformation("Пользователь {Email} успешно зарегистрирован", user.Email);
            return Ok(new { message = "Регистрация успешна", user.UserId });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Критическая ошибка при регистрации пользователя {Email}", registerDto.Email);
            return StatusCode(500, "Внутренняя ошибка сервера при регистрации");
        }
    }

    [HttpPut("user")]
    public async Task<IActionResult> EditUser([FromBody] UserDto userDto)
    {
        _logger.LogInformation("Запрос на обновление пользователя: {Email}", userDto.Email);
        
        var user = userDto.MapUserDtoToUser();
        var result = await userManageService.Update(user);
        
        if (result)
            return Ok(new { message = "Пользователь обновлен" });
            
        _logger.LogWarning("Не удалось обновить пользователя: {Email} (не найден)", userDto.Email);
        return NotFound(new { message = "Пользователь не найден" });
    }

    [HttpDelete("user/{userId}")]
    public async Task<IActionResult> DeleteUser([FromRoute] int userId)
    {
        _logger.LogInformation("Запрос на удаление пользователя с ID: {UserId}", userId);
        
        if (await userManageService.Delete(userId))
            return Ok(new { message = "Пользователь удален" });
            
        _logger.LogWarning("Попытка удаления несуществующего пользователя с ID: {UserId}", userId);
        return NotFound(new { message = "Пользователь не найден" });
    }
}