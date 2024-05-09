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
                    Id = Guid.Parse("bb4a3fac-1d7d-4705-bac8-4d0f8e546042"),
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
                    Id = Guid.Parse("e7d7bce8-2f38-409e-809d-d9692bffb20c"),
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
                    Id = Guid.Parse("6787607a-7c57-4832-8a31-e57a9aa59c0b"),
                    User = new HelpersDTO.Base.Models.BaseUserDTO()
                    {
                        LastName = "Сидоров",
                        FirstName = "Семен",
                        MiddleName = "Семенович"
                    },
                    Specialty = "Терапевт"
                },
        };
    }
}
