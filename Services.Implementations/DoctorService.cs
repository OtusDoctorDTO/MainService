using HelpersDTO.Doctor.DTO.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Abstractions;

namespace Services.Implementations
{
    public class DoctorService : IDoctorService
    {
        private readonly ILogger<DoctorService> _logger;
        private readonly IApplicationConfig _config;
        public DoctorService(IApplicationConfig config, ILogger<DoctorService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<FullInfoDoctorDTO?> GetById(Guid id)
        {
            try
            {
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
    }
}
