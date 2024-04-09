namespace MainServiceWebApi.Configs
{
    public interface IApplicationConfig
    {
        RabbitMqConfig BusConfig { get; set; }
    }
}
