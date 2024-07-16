using HelpersDTO.AppointmentDto.DTO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Abstractions;
using System.Net.Http.Json;

namespace Services.Implementations
{
    public class AppointmentService : IAppointmentService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IApplicationConfig _config;
        public AppointmentService(IApplicationConfig config, ILogger<MainService> logger)
        {
            _config = config;
            _logger = logger;
        }

        /// <summary>
        /// Обновить статус записи
        /// </summary>
        /// <param name="id">id записи</param>
        /// <param name="status">новый статус</param>
        /// <param name="userId">id пациента (опционально)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<bool> UpdateStatusAsync(Guid id, int status, Guid? userId = null)
        {
            try
            {
                var url = $"{_config.AppointnmentHost}/api/Home/UpdateStatus";
                var client = new HttpClient();
                var app = new UpdateStatusAppointmentDto()
                {
                    Id = id,
                    Status = status
                };
                if (userId != null)
                    app.PatientId = userId!.Value;
                var json = JsonConvert.SerializeObject(app);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(json, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                if (response == null)
                    throw new ArgumentNullException("Не пришел ответ");
                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при авторизации: {e}", e);
            }
            return false;
        }

        public async Task<AppointmentDto?> GetById(Guid id)
        {
            try
            {
                var url = $"{_config.AppointnmentHost}/api/Home/GetAppointmentById={id}";
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                if (response == null)
                    throw new ArgumentNullException("Не пришел ответ");
                return await response.Content.ReadFromJsonAsync<AppointmentDto?>();
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при авторизации: {e}", e);
            }
            return null;
        }

        public async Task<bool> BookAppointmentAsync(AppointmentDto appointmentDto)
        {
            try
            {
                var url = $"{_config.AppointnmentHost}/api/Home/AddAppointment";
                var client = new HttpClient();

                var json = JsonConvert.SerializeObject(appointmentDto);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (response == null)
                    throw new ArgumentNullException("Не пришел ответ");

                response.EnsureSuccessStatusCode();

                return await response.Content.ReadFromJsonAsync<bool>();
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при бронировании записи: {e}", e);
            }
            return false;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByPatientIdAsync(Guid patientId)
        {
            try
            {
                var url = $"{_config.AppointnmentHost}/api/Home/GetByPatientId?patientId={patientId}";
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode(); // Ensure success before reading content

                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonConvert.DeserializeObject<List<AppointmentDto>>(json);

                return appointments;
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при авторизации: {e}", e);
            }
            return null;
        }

        public async Task<List<AppointmentDto>> GetAppointmentsByDoctorIdAsync(Guid doctorId)
        {
            try
            {
                var url = $"{_config.AppointnmentHost}/api/Home/GetAppointmentsByDoctorId?doctorId={doctorId}";
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode(); // Ensure success before reading content

                var json = await response.Content.ReadAsStringAsync();
                var appointments = JsonConvert.DeserializeObject<List<AppointmentDto>>(json);

                return appointments;
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при авторизации: {e}", e);
            }
            return null;
        }
    }
}
