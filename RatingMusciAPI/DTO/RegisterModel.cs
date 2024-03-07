using System.ComponentModel.DataAnnotations;

namespace RatingMusciAPI.DTO;

public class RegisterModel
{
    [Required(ErrorMessage = "UserName is required!")]
    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "The email provided must be valid !")]
    [Required(ErrorMessage = "Email is required!")]
    public string? Email {  get; set; }

    [Required(ErrorMessage = "password is required!")]

    public string Password { get; set; }

}
