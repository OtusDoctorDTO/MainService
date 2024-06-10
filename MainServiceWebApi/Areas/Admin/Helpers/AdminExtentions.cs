using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Areas.Admin.Models;
using MainServiceWebApi.Helpers;

namespace MainServiceWebApi.Areas.Admin.Helpers
{
    public static class AdminExtentions
    {
        public static AppointmentViewModel? ToAppointmentViewModel(this ShortAppointnmentDTO? appointment, PatientDTO? patient)
        {
            if (appointment == null) return null;
            return new AppointmentViewModel()
            {
                Id = appointment!.Id,
                Price = appointment!.Price,
                StartDate = appointment!.StartDate,
                Date = appointment!.Date,
                Time = appointment!.Time,
                Status = appointment.Status ?? "",
                Patient = patient?.ToPatientVM()
                // доктор
            };
        }

        public static AppointmentFullInfoViewModel? ToAppointmentFullInfoVM(this AppointmentDto? appointment)
        {
            if (appointment == null) return null;
            return new AppointmentFullInfoViewModel()
            {
                Id = appointment.Id,
                Price = appointment.Price,
                Date = appointment.Date,
                Time = appointment.Time,
                Status = appointment.Status ?? ""
            };
        }
    }
}
