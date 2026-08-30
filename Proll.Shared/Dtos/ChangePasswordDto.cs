using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class ChangePasswordDto
{
    [Required]
    public string NewPassword { get; set; }
    [Required]
    public string CurrentPassword { get; set; }
    [JsonIgnore]
    [Required, Compare(nameof(NewPassword))]
    public string ConfrimNewPasswordd { get; set; }

}