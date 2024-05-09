using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.Doctor.DTO.Models;
using MainServiceWebApi.Models;

namespace MainServiceWebApi.Helpers
{
    public static class Extentions
    {
        public static DoctorViewModel? ToDoctorVM(this DoctorDTO doctor)
        {
            if (doctor == null) return null;
            return new DoctorViewModel()
            {
                Id = doctor.Id,
                FirstName = doctor.User.FirstName,
                LastName = doctor.User.LastName,
                MiddleName = doctor.User.MiddleName,
                Specialty = doctor.Specialty,
            };
        }

        public static ShortAppointnmentViewModel? ToAppointmentVM(this ShortAppointnmentDTO appointnment)
        {
            if (appointnment == null) return null;
            return new ShortAppointnmentViewModel()
            {
                Id = appointnment.Id,
            };
        }
    }
}
