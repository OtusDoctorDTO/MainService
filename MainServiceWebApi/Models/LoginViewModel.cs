using System.ComponentModel.DataAnnotations;

namespace MainServiceWebApi.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Введите электронную почту")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Неверный формат электронной почты")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(35, MinimumLength = 4, ErrorMessage = "Пароль должен быть минимум 4 символа, максимум 35 символов")]
        public string? Password { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
