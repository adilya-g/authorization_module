using System.ComponentModel.DataAnnotations;

namespace ExampleApplication.DTO;

public class RegisterDto
{
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string LastName { get; set; }
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string Email { get; set; }
    [Required]
    [StringLength(255, MinimumLength = 6)]
    public string RawPassword { get; set; }
}