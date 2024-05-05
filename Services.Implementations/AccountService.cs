using Domain.Entities.Configs;
using HelpersDTO.Authentication;
using HelpersDTO.Authentication.DTO.Models;
using HelpersDTO.Doctor.DTO.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Abstractions;

namespace Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ILogger<MainService> _logger;
        private readonly IApplicationConfig _config;
        public AccountService(IApplicationConfig config, ILogger<MainService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<LoginResponse?> LoginAsync(LoginDTO login)
        {
            try
            {
                var url = $"{_config.AuthHost}/User/Login";
                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:7249/User/Login");
                var content = new StringContent(JsonConvert.SerializeObject(login), null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<LoginResponse>(data);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при авторизации: {e}", e);
            }
            return null;
        }

        public Task<RegistrationResponse> RegisterAsync(RegisterDTO register)
        {
            throw new NotImplementedException();
        }
    }
}
