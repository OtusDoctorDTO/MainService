namespace MainServiceWebApi.Models
{
    public class ShortAppointmentViewModel
    {
        public DateOnly Date { get; set; }
        public Dictionary<Guid, TimeOnly> Data { get; set; } = new();
    }
}
