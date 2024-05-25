using HelpersDTO.Doctor.DTO.Models;

namespace Domain.Entities
{
    public static class Constants
    {
        public static readonly string User = "User";
        public static readonly string Admin = "Admin";

        public static List<DoctorDTO> BaseDoctors = new List<DoctorDTO>()
        {
            new()
            {
                Id = Guid.Parse("61dd8fe5-aed3-44af-a451-823caeb2dc68"),
                User = new HelpersDTO.Base.Models.BaseUserDTO()
                {
                    LastName = "Иванов",
                    FirstName = "Иван",
                    MiddleName = "Иванович"
                },
                Specialty = "Терапевт"
            },
            new()
            {
                Id = Guid.Parse("c302105d-1c7e-4db4-9380-d91146596f05"),
                User = new HelpersDTO.Base.Models.BaseUserDTO()
                {
                    LastName = "Петров",
                    FirstName = "Петр",
                    MiddleName = "Петрович"
                },
                Specialty = "Терапевт"
            },
            new()
            {
                Id = Guid.Parse("eec214ba-7605-4392-9ec4-e788ded53bea"),
                User = new HelpersDTO.Base.Models.BaseUserDTO()
                {
                    LastName = "Сидоров",
                    FirstName = "Семен",
                    MiddleName = "Семенович"
                },
                Specialty = "Терапевт"
            },

            new()
            {
                Id = Guid.Parse("a372bf3d-efe9-4f0d-b16b-ac550a3947d2"),
                User = new HelpersDTO.Base.Models.BaseUserDTO()
                {
                    LastName = "Кузнецов",
                    FirstName = "Иван",
                    MiddleName = "Петрович"
                },
                Specialty = "Терапевт"
            },
            new()
            {
                Id = Guid.Parse("c5508ee0-fbe6-47fa-b7d3-d6438b93b43d"),
                User = new HelpersDTO.Base.Models.BaseUserDTO()
                {
                    LastName = "Яковлев",
                    FirstName = "Алексай",
                    MiddleName = "Иванович"
                },
                Specialty = "Терапевт"
            },
        };
    };
}
