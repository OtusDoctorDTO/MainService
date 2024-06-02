using HelpersDTO.Patient;
using HelpersDTO.Patient.DTO;

namespace Services.Abstractions
{
    public interface IPatientService
    {
        Task<bool> AddPatientAsync(PatientDTO patient);
        Task<PatientDTO> GetPatientProfileAsync(Guid userId);
    }
}
