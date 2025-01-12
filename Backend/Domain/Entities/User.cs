namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public virtual Cart Cart { get; set; }

        public User()
        {
        }

        public User(Guid id, string username, string password, string email, string? phoneNumber = null)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
        }
    }
}
