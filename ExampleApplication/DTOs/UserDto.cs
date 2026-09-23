using System.ComponentModel.DataAnnotations;

namespace ExampleApplication.DTO;

public class UserDto
{
    public int? UserId { get; set; }
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string LastName { get; set; }
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string Email { get; set; }
}