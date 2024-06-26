﻿using System.ComponentModel.DataAnnotations;

namespace MainServiceWebApi.Models
{
    public class PassportViewModel
    {
        /// <summary>
        /// Серия
        /// </summary>
        [Required(ErrorMessage = "Введите серию")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Длина строки должна быть 4 символа")]
        public string? Series { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        [Required(ErrorMessage = "Введите номер")]
        [StringLength(6, MinimumLength = 6, ErrorMessage = "Длина строки должна быть 6 символов")]
        public string? Number { get; set; }

        /// <summary>
        /// Дата выдачи
        /// </summary>
        [Required(ErrorMessage = "Введите дату выдачи")]
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Код подразделения
        /// </summary>
        [Required(ErrorMessage = "Введите код подразделения")]
        [RegularExpression(@"^\d{3}\-\d{3}$", ErrorMessage = "Некорректный формат кода подразделения")]
        public string? SubdivisionCode { get; set; }

        /// <summary>
        /// Кем выдан
        /// </summary>
        [Required(ErrorMessage = "Введите кем выдано")]
        public string? IssuedBy { get; set; }
        
    }
}
