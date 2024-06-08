using HelpersDTO.AppointmentDto.DTO;
using HelpersDTO.Authentication;
using HelpersDTO.Doctor.DTO.Models;
using HelpersDTO.Patient.DTO;
using MainServiceWebApi.Areas.Admin.Models;
using MainServiceWebApi.Models;
using System.Globalization;

namespace MainServiceWebApi.Helpers
{
    public static class Extentions
    {
        private static readonly int dayInWeek = 7;
        private static readonly int countOfWeeks = 4;
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
            // получение расписания
            var fourWeeksByCurrentDay = new List<WeekScheduleInfo>();
            for (int i = 0; i < countOfWeeks; i++)
            {
                var week = GetWeekDays(now.AddDays(i * dayInWeek));
                fourWeeksByCurrentDay.Add(new WeekScheduleInfo()
                {
                    DayOfWeekInfos = week.Select(day => new DayOfWeekInfo()
                    {
                        Date = DateOnly.FromDateTime(day),
                        ForTime = (doctor.IntervalInfo?.ContainsKey(DateOnly.FromDateTime(day)) ?? false) ?
                        doctor.IntervalInfo?[DateOnly.FromDateTime(day)]?.ForTime : null,
                        SinceTime = (doctor.IntervalInfo?.ContainsKey(DateOnly.FromDateTime(day)) ?? false) ?
                        doctor.IntervalInfo?[DateOnly.FromDateTime(day)]?.SinceTime : null,
                    }).ToList()
                });
            }
            return new FullDoctorInfoViewModel()
            {
                Id = doctor.Id!.Value,
                FirstName = doctor.UserInfo?.FirstName ?? "",
                LastName = doctor.UserInfo?.LastName ?? "",
                MiddleName = doctor.UserInfo?.MiddleName,
                Experience = doctor.StartWorkDate!.Value.GetAge(now),
                Cabinet = doctor.Cabinet,
                Specialty = null,
                WeekScheduleInfos = fourWeeksByCurrentDay
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

        /// <summary>
        /// По дате получить все дни недели с учетом культуры
        /// </summary>
        /// <param name="dateValue"></param>
        /// <returns></returns>
        public static List<DateTime> GetWeekDays(DateTime dateValue)
        {
            var culture = CultureInfo.CurrentCulture;
            var weekOffset = culture.DateTimeFormat.FirstDayOfWeek - dateValue.DayOfWeek;
            var startOfWeek = dateValue.AddDays(weekOffset);
            return Enumerable.Range(0, 7).Select(i => startOfWeek.AddDays(i)).ToList();
        }

        public static PatientViewModel? ToPatientVM(this PatientDTO? patient)
        {
            if (patient == null) return null;
            return new PatientViewModel()
            {
                FirstName = patient.FirstName ?? "",
                LastName = patient.LastName ?? "",
                Email = patient.Email ?? "",
                PhoneNumber = patient.Phone ?? "",
                IsNew = patient.IsNew
            };
        }
    }
}
