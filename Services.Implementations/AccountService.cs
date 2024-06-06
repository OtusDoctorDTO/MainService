using HelpersDTO.Authentication;
using HelpersDTO.Authentication.DTO.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Abstractions;
using System.Net.Http.Json;

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
                var json = JsonConvert.SerializeObject(login);
                var request = new HttpRequestMessage(HttpMethod.Post, url);
                var content = new StringContent(json, null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                if (response == null)
                    throw new ArgumentNullException("Не пришел ответ");
                return await response.Content.ReadFromJsonAsync<LoginResponse>();
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при авторизации: {e}", e);
            }
            return null;
        }

        public async Task<RegistrationResponse?> RegisterAsync(RegisterDTO register)
        {
            var url = $"{_config.AuthHost}/User/Register";
            var client = new HttpClient();
            var json = JsonConvert.SerializeObject(register);
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            var content = new StringContent(json, null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            if (response == null)
                throw new ArgumentNullException("Не пришел ответ");
            var result = await response!.Content.ReadFromJsonAsync<RegistrationResponse?>();
            return result;
        }
    }
}
