using MainServiceWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MainServiceWebApi.Areas.Admin.Models
{
    public class CheckPatientViewModel
    {
        //добавить данные заполненные из записи ко врачу
        public Guid? UserId { get; set; }
        public Guid? AppointmentId { get; set; }
        [Required(ErrorMessage = "Введите фамилию")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Введите имя")]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public PassportViewModel Passport { get; set; } = new();
        public CheckPatientViewModel()
        {
        }
    }
}
