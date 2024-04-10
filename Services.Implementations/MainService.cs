using Domain.Entities.Configs;
using HelpersDTO.Doctor.DTO.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Services.Abstractions;
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

        public async Task<List<DoctorDTO>> GetDoctors()
        {
            try
            {
                var url = $"{_config.DoctorHost}/api/Doctor";

                var client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var data = await response.Content.ReadAsStringAsync();
                var doctors = JsonConvert.DeserializeObject<List<DoctorDTO>>(data);
                return doctors;
            }
            catch (Exception e)
            {
                _logger.LogError("Произошла ошибка при получении данных из Doctor: {e}", e);
            }
            return null;
            return new List<DoctorDTO>()
            {
                new()
                { 
                    Id = Guid.Parse("bb4a3fac-1d7d-4705-bac8-4d0f8e546042"),
                    User = new HelpersDTO.Base.Models.BaseUserDTO()
                    {
                        LastName = "Иванов",
                        FirstName = "Иван",
                        MiddleName = "Иванович"
                    },
                    Specialty = "Терапевт"
                },
                new()
                {
                    Id = Guid.Parse("e7d7bce8-2f38-409e-809d-d9692bffb20c"),
                    User = new HelpersDTO.Base.Models.BaseUserDTO()
                    {
                        LastName = "Петров",
                        FirstName = "Петр",
                        MiddleName = "Петрович"
                    },
                    Specialty = "Терапевт"
                },
                new()
                {
                    Id = Guid.Parse("6787607a-7c57-4832-8a31-e57a9aa59c0b"),
                    User = new HelpersDTO.Base.Models.BaseUserDTO()
                    {
                        LastName = "Сидоров",
                        FirstName = "Семен",
                        MiddleName = "Семенович"
                    },
                    Specialty = "Терапевт"
                },
            };
        }
    }
}
