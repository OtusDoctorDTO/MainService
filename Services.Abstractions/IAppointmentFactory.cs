using HelpersDTO.AppointmentDto.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public interface IAppointmentFactory
    {
        AppointmentDto CreateAppointment(Guid patientId, Guid doctorId, DateOnly date, TimeOnly time);
        Task BookAppointmentAsync(AppointmentDto appointmentDto);
    }
}
