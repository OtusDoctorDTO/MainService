using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.Authentication;
using HelpersDTO.Doctor.DTO.Models;

namespace Services.Abstractions
{
    public interface IMainService
    {
        Task<List<DoctorDTO>?> GetDoctors();
        Task<List<ShortAppointnmentDTO>?> GetActiveAppointnmentsAsync(ShortAppointmentRequest appointmentRequest);
        Task<List<DoctorDTO>?> GetDoctorsByIds(Guid[] ids);
    }
}
