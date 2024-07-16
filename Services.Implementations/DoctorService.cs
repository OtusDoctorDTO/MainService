using HelpersDTO.Doctor.DTO.Models;
using HelpersDTO.Patient.DTO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Abstractions;
using System.Net.Http;

namespace Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly ILogger<DoctorService> _logger;
        private readonly IApplicationConfig _config;
        private readonly HttpClient _httpClient;
        public DoctorService(IApplicationConfig config, ILogger<DoctorService> logger, HttpClient httpClient)
        {
            _config = config;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task<bool> AddDoctorAsync(NewDoctorDTO doctor)
        {
            var url = $"{_config.DoctorHost}/api/Doctor/Add";
            var json = JsonConvert.SerializeObject(doctor);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);
            if (response == null)
                throw new ArgumentNullException("Не пришел ответ");

            return response.IsSuccessStatusCode;
        }

        public async Task<FullInfoDoctorDTO?> GetById(Guid? id)
        {
            try
            {
                if (id == null) return null;
                var url = $"{_config.DoctorHost}/api/Doctor/GetFullInfoById?id={id}";
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var client = new HttpClient();
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<FullInfoDoctorDTO>(data);
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении данных из Doctor: {e}", e);
            }
            return null;
        }

        public async Task<List<DoctorDTO>> GetAllDoctorsAsync()
        {
            try
            {
                var url = $"{_config.DoctorHost}/api/Doctor/AllDoctors";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var client = new HttpClient();
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<List<DoctorDTO>>(data);
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении данных из Doctor: {e}", e);
            }
            return null;
        }

    }
}
