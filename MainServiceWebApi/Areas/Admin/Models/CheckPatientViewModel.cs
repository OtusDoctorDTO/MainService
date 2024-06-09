using MainServiceWebApi.Models;
using System.ComponentModel.DataAnnotations;

namespace MainServiceWebApi.Areas.Admin.Models
{
    public class CheckPatientViewModel
    {
        //добавить данные заполненные из записи ко врачу
        public Guid? UserId { get; }
        public Guid? AppointmentId { get; }
        [Required(ErrorMessage = "Введите фамилию")]
        public string? LastName { get; set; }
        [Required(ErrorMessage = "Введите имя")]
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public PassportViewModel Passport { get; set; } = new();

        public CheckPatientViewModel(Guid userId, Guid appoitmentid)
        {
            UserId = userId;
            AppointmentId = appoitmentid;
            Passport= new();
        }
        public CheckPatientViewModel()
        {
        }
    }
}
