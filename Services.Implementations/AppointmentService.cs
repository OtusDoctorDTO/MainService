using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.AppointmentDto.Enums;
using HelpersDTO.Authentication;
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

        public async Task<bool> UpdateStatusAsync(Guid id, Guid userId, int success)
        {
            try
            {
                var url = $"{_config.AppointnmentHost}/api/Home/UpdateStatus";
                var client = new HttpClient();
                var app = new UpdateStatusAppointmentDto()
                {
                    Id = id,
                    Status = (int)StatusEnum.Success,
                    PatientId = userId
                };
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
    }
}
