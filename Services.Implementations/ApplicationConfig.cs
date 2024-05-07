using Services.Abstractions;

namespace Services.Implementations
{
    public class ApplicationConfig: IApplicationConfig
    {
        public RabbitMqConfig BusConfig { get; set; } = default!;
        public string DoctorHost { get; set; } = default!;
        public string AuthHost { get; set; } = default!;
        public AuthOptions AuthOptions { get; set; } = default!;
    }
}
