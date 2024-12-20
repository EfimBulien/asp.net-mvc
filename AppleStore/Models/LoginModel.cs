using System.ComponentModel.DataAnnotations;

namespace AppleStore.Models;

public class LoginModel
{
    [Required(ErrorMessage = "Введите логин")]
    public string Login { get; set; }
    [Required(ErrorMessage = "Введите пароль")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}