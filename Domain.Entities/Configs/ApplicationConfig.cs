namespace Domain.Entities.Configs
{
    public class ApplicationConfig: IApplicationConfig
    {
        public RabbitMqConfig BusConfig { get; set; } = default!;
        public string DoctorHost { get; set; } = default!;
        public string AuthHost { get; set; } = default!;
    }
}
