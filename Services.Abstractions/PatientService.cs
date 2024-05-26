using Domain.Entities;
using System.Net.Http.Json;

namespace Services.Abstractions
{
    public class PatientService
    {
        private readonly HttpClient _httpClient;

        public PatientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task AddPatientAsync(PatientDTO patient)
        {
            var response = await _httpClient.PostAsJsonAsync("api/patients", patient);
            response.EnsureSuccessStatusCode();
        }
        public async Task<PatientDTO> GetPatientProfileAsync(Guid userId)
        {
            // Логика для получения профиля пациента из репозитория по его идентификатору userId
            // Пример:
            var patient = new PatientDTO {
                FirstName = "Иван",
                LastName = "Иванов",
                DateOfBirth = Convert.ToDateTime(21/12/1998),
                PhoneNumber = "891234567"
            };
            return patient;
        }
    }
}