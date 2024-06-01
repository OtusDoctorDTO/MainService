using HelpersDTO.AppointmentDto.DTO;

namespace Services.Abstractions
{
    public interface IAppointmentService
    {
        Task<AppointmentDto?> GetById(Guid id);
        Task<bool> UpdateStatusAsync(Guid id, Guid userId, int success);
    }
}
