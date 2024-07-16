using HelpersDTO.AppointmentDto.DTO;

namespace Services.Abstractions
{
    public interface IAppointmentService
    {
        Task<AppointmentDto?> GetById(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, int status, Guid? userId = null);
        Task<bool> BookAppointmentAsync(AppointmentDto appointmentDto);
        Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(Guid patientId);
        Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId);
    }
}
