using Proll.Shared.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.Marshalling;

public class LoginDto
{
    [Required]
    public string Username  { get; set; }
    [Required]
    public string Password { get; set; }
}
