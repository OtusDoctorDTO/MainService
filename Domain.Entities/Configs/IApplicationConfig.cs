namespace Domain.Entities.Configs
{
    public interface IApplicationConfig
    {
        RabbitMqConfig BusConfig { get; set; }
        string DoctorHost { get; set; }
        string AuthHost { get; set; }
    }
}
