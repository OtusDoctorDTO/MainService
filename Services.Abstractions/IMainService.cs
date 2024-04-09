using HelpersDTO.Doctor.DTO.Models;

namespace Services.Abstractions
{
    public interface IMainService
    {
        Task<List<DoctorDTO>> GetDoctors();
    }
}
