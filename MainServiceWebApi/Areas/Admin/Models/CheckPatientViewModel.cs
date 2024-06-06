using MainServiceWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MainServiceWebApi.Areas.Admin.Models
{
    public class CheckPatientViewModel
    {
        //добавить данные заполненные из записи ко врачу
        public Guid? UserId { get; }
        public Guid? AppointmentId { get; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        /// <summary>
        /// Тип документа (по умолчанию - паспорт)
        /// </summary>
        public int DocumentType { get; set; } = 1;
        public PassportViewModel? Passport { get; set; } = new();

        /// <summary>
        /// Пол
        /// </summary>
        [Required]
        [Range(1, 2)]
        public int? Gender { get; set; }

        public CheckPatientViewModel(Guid userId, Guid appoitmentid)
        {
            UserId = userId;
            AppointmentId = appoitmentid;
        }
    }
}
