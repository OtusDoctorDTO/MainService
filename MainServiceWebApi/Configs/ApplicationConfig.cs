namespace MainServiceWebApi.Configs
{
    public class ApplicationConfig : IApplicationConfig
    {
        public RabbitMqConfig BusConfig { get; set; } = default!;
    }
}
