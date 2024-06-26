﻿using Services.Abstractions;

namespace Services.Implementations
{
    public class ApplicationConfig: IApplicationConfig
    {
        public RabbitMqConfig BusConfig { get; set; } = default!;
        public string DoctorHost { get; set; } = default!;
        public string AuthHost { get; set; } = default!;
        public string AppointnmentHost { get; set; } = default!;
        public string PatientHost { get; set; } = default!;
        public AuthOptions AuthOptions { get; set; } = default!;
        public string CookiesName { get; set; } = default!;
    }
}
