using System.ComponentModel.DataAnnotations;

namespace ExampleApplication.DTO;

public class LoginDto
{
    [Required]
    [StringLength(254, MinimumLength = 3)]
    public string email { get; set; }
    [Required]
    [StringLength(255, MinimumLength = 6)]
    public string password { get; set; }
}