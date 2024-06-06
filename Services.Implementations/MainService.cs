using Domain.Entities;
using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.Authentication;
using HelpersDTO.Doctor.DTO.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using Newtonsoft.Json;
using Services.Abstractions;
using System.Net.Http.Json;
using System.Text;

namespace Services.Implementations
{
    public class MainService : IMainService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IApplicationConfig _config;
        public MainService(IApplicationConfig config, ILogger<MainService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<List<ShortAppointnmentDTO>?> GetActiveAppointnmentsAsync(ShortAppointmentRequest appointmentRequest)
        {
            try
            {
                var url = $"{_config.AppointnmentHost}/api/Home/GetAppointments";
                var client = new HttpClient();
                var json = JsonConvert.SerializeObject(appointmentRequest);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(json, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                if (response == null)
                    throw new ArgumentNullException("Не пришел ответ");
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var appointments = JsonConvert.DeserializeObject<List<ShortAppointnmentDTO>>(data);
                return appointments;
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении доступных записей: {message}", e);
            }
            return null;
        }

        public async Task<List<DoctorDTO>?> GetDoctorsByIds(Guid[] ids)
        {
            try
            {
                var url = $"{_config.DoctorHost}/api/Doctor/GetDoctorsByIds";
                var json = JsonConvert.SerializeObject(ids);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(json, null, "application/json");
                request.Content = content;
                var client = new HttpClient();
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var doctors = JsonConvert.DeserializeObject<List<DoctorDTO>>(data);
                if (doctors?.Any() ?? false)
                    return doctors;
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении данных из Doctor: {e}", e);
            }
            return null;
        }
    }
}
