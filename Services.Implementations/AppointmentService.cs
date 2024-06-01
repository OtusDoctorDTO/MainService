using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.AppointmentDto.Enums;
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
                var url = $"{_config.AppointnmentHost}/api/Home/GetAppointmentById?id={id}";
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
    }
}
