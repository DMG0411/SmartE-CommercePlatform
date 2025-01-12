namespace Application.DTOs.User
{
    public class UserDTO : CreateUserDTO
    {
        public Guid? Id { get; set; }

        public string? PhoneNumber { get; set; }

        public string? City { get; set; }

        public UserDTO() : base()
        {
        }

        public UserDTO(Guid id, string username, string password, string email, string? phoneNumber, string? city) : base(username, password, email)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            City = city;
        }
    }
}
