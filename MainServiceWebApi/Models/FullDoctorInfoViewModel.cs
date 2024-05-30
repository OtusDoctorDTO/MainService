namespace MainServiceWebApi.Models
{
    public class FullDoctorInfoViewModel
    {
        public Guid Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string? MiddleName { get; set; }
        public string? Specialty { get; set; }
        public int? Experience { get; set; }
        public string? Cabinet { get; set; } = string.Empty;
        public List<WeekScheduleInfo>? WeekScheduleInfos { get; set; }
    }
}
