namespace Application.DTOs.User
{
    public class CreateUserDTO : LoginUserDTO
    {
        public string Username { get; set; }


        public CreateUserDTO(string username, string password, string email)
            : base(username, password)
        {
            Username = username;
        }

        public CreateUserDTO()
        {
        }
    }
}
