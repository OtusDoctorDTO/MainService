using HelpersDTO.Doctor.DTO.Models;

namespace Services.Abstractions
{
    public interface IDoctorService
    {
        Task<FullInfoDoctorDTO?> GetById(Guid id);
    }
}
