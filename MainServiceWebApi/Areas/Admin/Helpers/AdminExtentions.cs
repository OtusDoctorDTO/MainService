using HelpersDTO.AppointmentDto.DTO;
using MainServiceWebApi.Areas.Admin.Models;

namespace MainServiceWebApi.Areas.Admin.Helpers
{
    public static class AdminExtentions
    {
        public static AppointmentViewModel? ToAppointmentViewModel(this ShortAppointnmentDTO? appointment)
        {
            if (appointment == null) return null;
            return new AppointmentViewModel()
            {
                Id = appointment!.Id,
                Price = appointment!.Price,
                StartDate = appointment!.StartDate,
                Date = appointment!.Date,
                Time = appointment!.Time,
                Status = appointment.Status ?? ""
                //PatientId = 
            };
        }


        public static AppointmentFullInfoViewModel? ToAppointmentFullInfoVM(this AppointmentDto? appointment)
        {
            if(appointment == null) return null;
            return new AppointmentFullInfoViewModel()
            {
                Id = appointment.Id,
                Price = appointment.Price,
                StartDate = appointment.Time,
                Status = appointment.Status ?? ""
            };
        }
    }
}
