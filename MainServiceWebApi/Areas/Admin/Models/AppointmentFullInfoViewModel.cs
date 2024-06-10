using MainServiceWebApi.Models;

namespace MainServiceWebApi.Areas.Admin.Models
{
    public class AppointmentFullInfoViewModel
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        /// <summary>
        /// дата начала приема
        /// </summary>
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string? Status { get; set; } = string.Empty;
        public FullDoctorInfoViewModel? Doctor { get; set; }
        public PatientViewModel? Patient  { get; set; }
    }
}
