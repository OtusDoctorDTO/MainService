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

        public static FullDoctorInfoViewModel? ToFullDoctorInfoVM(this FullInfoDoctorDTO doctor, DateTime now)
        {
            if (doctor == null) return null;
            return new FullDoctorInfoViewModel()
            {
                Id = doctor.Id!.Value,
                FirstName = doctor.UserInfo?.FirstName ?? "",
                LastName = doctor.UserInfo?.LastName ?? "",
                MiddleName = doctor.UserInfo?.MiddleName,
                Experience = doctor.StartWorkDate!.Value.GetAge(now),
                Cabinet = doctor.Cabinet,

                // у текущей даты найти день недели
                // найти сколько дней до понедельника
                // заполнить график на 4 недели
                // добавить данные из доктора по графику
                //WeekScheduleInfos = 

                //Specialty = doctor.,
            };
        }



        /// <summary>
        /// Получить стаж
        /// </summary>
        /// <param name="startWorkDate">дата начала трудовой деятельности</param>
        /// <param name="now">текущая дата</param>
        /// <returns></returns>
        public static int GetAge(this DateTime startWorkDate, DateTime now)
        {
            var r = now.Year - startWorkDate.Year;
            return startWorkDate.AddYears(r) <= now ? r : r - 1;
        }
    }
}
