namespace MainServiceWebApi.Models
{
    public class MainPageViewModel
    {
        public Guid? DoctorId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public List<ShortAppointmentViewModel> Appointments { get; set; } = new();
    }
}
