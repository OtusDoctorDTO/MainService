namespace MainServiceWebApi.Models
{
    public class PatientViewModel
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public Guid? UsersId { get; set; }
    }
}
