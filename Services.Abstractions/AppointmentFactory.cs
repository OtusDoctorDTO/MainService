using HelpersDTO.AppointmentDto.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstractions
{
    public class AppointmentFactory : IAppointmentFactory
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentFactory(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        public AppointmentDto CreateAppointment(Guid patientId, Guid doctorId, DateOnly date, TimeOnly time)
        {
            return new AppointmentDto
            {
                PatientId = patientId,
                DoctorId = doctorId,
                Date = date,
                Time = time
            };
        }

        public async Task BookAppointmentAsync(AppointmentDto appointmentDto)
        {
            await _appointmentService.BookAppointmentAsync(appointmentDto);
        }
    }
}
