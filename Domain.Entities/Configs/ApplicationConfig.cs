using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Configs
{
    public class ApplicationConfig: IApplicationConfig
    {
        public RabbitMqConfig BusConfig { get; set; } = default!;
        public string DoctorHost { get; set; } = default!;
    }
}
