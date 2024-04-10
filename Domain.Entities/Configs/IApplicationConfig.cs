using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Configs
{
    public interface IApplicationConfig
    {
        RabbitMqConfig BusConfig { get; set; }
        string DoctorHost { get; set; }
    }
}
