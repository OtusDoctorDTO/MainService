namespace MainServiceWebApi.Models
{
    public class DayOfWeekInfo
    {
        public DateOnly Date { get; set; }
        public TimeOnly? SinceTime { get; set; }
        public TimeOnly? ForTime { get; set; }
    }
}
