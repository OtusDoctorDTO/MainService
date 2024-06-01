namespace Services.Abstractions
{
    public interface IApplicationConfig
    {
        RabbitMqConfig BusConfig { get; set; }
        string DoctorHost { get; set; }
        string AuthHost { get; set; }
        string AppointnmentHost { get; set; }
        string PatientHost { get; set; }
        string CookiesName { get; set; }
        AuthOptions AuthOptions { get; set; }
    }
}
