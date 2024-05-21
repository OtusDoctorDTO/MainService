namespace MainServiceWebApi.Models
{
    public class ShortAppointmentViewModel
    {
        public DateTime? Date { get; set; }
        public Dictionary<Guid, TimeSpan> Data { get; set; } = new();
    }
}
