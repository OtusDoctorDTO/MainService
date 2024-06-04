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
            var url = $"{_config.PatientHost}/api/Patients/Add";
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(patient);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            if (response == null)
                throw new ArgumentNullException("Не пришел ответ");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<PatientDTO>?> GetByIdsAsync(Guid[] usersId)
        {
            var url = $"{_config.PatientHost}/api/Patients/GetByIds";
            var json = JsonConvert.SerializeObject(usersId);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var client = new HttpClient();
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var data = await response.Content.ReadAsStringAsync();
            var patients = JsonConvert.DeserializeObject<List<PatientDTO>>(data);
            return patients;
        }

        public async Task<PatientDTO?> GetPatientProfileAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"{_config.PatientHost}/Patients/{userId}");
            if (response == null)
                throw new ArgumentNullException("Не пришел ответ");

            if (!response.IsSuccessStatusCode)
            {
                return null; // или выбросьте исключение, если это необходимо
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseContent))
            {
                return null;
            }

            var patient = JsonConvert.DeserializeObject<PatientDTO>(responseContent);
            if (patient == null)
            {
                return null;
            }

            return patient;
        }
    }
}