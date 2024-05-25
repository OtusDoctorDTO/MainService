using HelpersDTO.Authentication;
using HelpersDTO.Authentication.DTO.Models;

namespace Services.Abstractions
{
    public interface IAppointmentService
    {
        Task<bool> UpdateStatusAsync(Guid id, Guid userId, int success);
    }
}
