using HelpersDTO.AppointmentDto.DTO;
using MainServiceWebApi.Areas.Admin.Models;

namespace MainServiceWebApi.Models
{
    public class PatientViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? UserId { get; set; }
        public bool IsNew { get; set; }

        // Для записи к врачу
        public Guid DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime AppointmentTime { get; set; }
        public List<DoctorViewModel> Doctors { get; set; }
        public bool? BookingSuccess { get; set; }
        public List<AppointmentViewModel> Appointments { get; set; }
        public List<AppointmentDto> SelectedDoctorAppointments { get; set; }
        public Guid SelectedDoctorId { get; set; }

    }
}
