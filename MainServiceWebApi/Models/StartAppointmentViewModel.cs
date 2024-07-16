namespace MainServiceWebApi.Models
{
    public class StartAppointmentViewModel
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public string? Complaints { get; set; }
        public string? Recommendations { get; set; }
    }
}
