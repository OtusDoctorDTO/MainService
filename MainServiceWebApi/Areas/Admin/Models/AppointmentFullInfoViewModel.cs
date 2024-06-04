﻿using MainServiceWebApi.Models;

namespace MainServiceWebApi.Areas.Admin.Models
{
    public class AppointmentFullInfoViewModel
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateOnly Date { get; set; }
        public TimeOnly Time { get; set; }
        public string Status { get; set; } = string.Empty;
        public bool IsNewPatient { get; set; }
        public FullDoctorInfoViewModel? Doctor { get; set; }
        public PatientViewModel? Patient  { get; set; }
    }
}