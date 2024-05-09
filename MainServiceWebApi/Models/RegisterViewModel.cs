using System.ComponentModel.DataAnnotations;

namespace MainServiceWebApi.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Введите электронную почту")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "Неверный формат электронной почты")]
        public string Email { get; set; }

        [StringLength(10, MinimumLength = 9, ErrorMessage = "Неверное количество символов")]
        [RegularExpression("\\(?\\d{3}\\)?-? *\\d{3}-? *-?\\d{4}", ErrorMessage = "Неверный формат телефона")]
        public string? Phone { get; set; }

        [Required(ErrorMessage = "Введите пароль"), DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен быть минимум 6 символов, максимум 20 символов")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите пароль"), DataType(DataType.Password), Compare(nameof(Password))]
        [StringLength(20, MinimumLength = 6, ErrorMessage = "Пароль должен быть минимум 6 символов, максимум 20 символов")]
        public string ConfirmPassword { get; set; }
        public bool RememberMe { get; set; } = true;
        public string? ReturnUrl { get; set; }
    }
}
