namespace Services.Abstractions
{
    public class RabbitMqConfig
    {
        public string Host { get; set; } = default!;
        public string Path { get; set; } = default!;
        public ushort Port { get; set; }
        public string Username { get; set; } = default!;
        public string Password { get; set; } = default!;
        public int FetchCount { get; set; }
    }
}
