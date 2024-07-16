using HelpersDTO.Doctor.DTO.Models;
using HelpersDTO.Patient.DTO;
using System.Numerics;

namespace Services.Abstractions
{
    public interface IDoctorService
    {
        Task<FullInfoDoctorDTO?> GetById(Guid? id);
        Task<bool> AddDoctorAsync(NewDoctorDTO doctor);
        Task<List<DoctorDTO>> GetAllDoctorsAsync();
    }
}
