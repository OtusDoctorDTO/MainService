using HelpersDTO.Patient.DTO;
using Newtonsoft.Json;

namespace Services.Abstractions
{
    public class PatientService : IPatientService
    {
        private readonly HttpClient _httpClient;
        private readonly IApplicationConfig _config;

        public PatientService(IApplicationConfig config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public async Task<bool> AddPatientAsync(PatientDTO patient)
        {
            var url = $"{_config.PatientHost}/Patients/Add";
            var json = JsonConvert.SerializeObject(patient);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (response == null)
                throw new ArgumentNullException("Не пришел ответ");

            return response.IsSuccessStatusCode;
        }

        public async Task<PatientDTO?> GetPatientProfileAsync(Guid userId)
        {
            var url = $"{_config.PatientHost}/Patients/{userId}";
            var response = await _httpClient.GetAsync(url);

            if (response == null)
                throw new ArgumentNullException("Не пришел ответ");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                return null;
            }

            var patient = JsonConvert.DeserializeObject<PatientDTO>(responseContent);
            return patient;
        }
    }
}