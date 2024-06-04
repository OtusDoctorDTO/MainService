using MainServiceWebApi.Models;

namespace MainServiceWebApi.Areas.Admin.Models
{
    public class AppointmentViewModel
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; } = string.Empty;
        public PatientViewModel? Patient { get; set; }
        //TODO добавить данные доктора?!
    }
}
