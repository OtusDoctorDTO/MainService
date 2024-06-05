using HelpersDTO.AppointmentDto.Enums;

namespace MainServiceWebApi.Areas.Admin.Models
{
    public class AppointmentPanelViewModel
    {

        public int Count { get; set; } = 20;
        public DateTime? StartDate { get; set; } = DateTime.Now.AddDays(-14);
        public DateTime? EndDate { get; set; } = DateTime.Now;
        public Dictionary<StatusEnum, bool>? Statuses { get; set; }
        public bool DESC { get; set; } = true;
        public List<AppointmentViewModel>? AppointmentsSearchResult { get; set; }
    }
}
