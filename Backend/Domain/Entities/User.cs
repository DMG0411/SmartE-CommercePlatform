namespace Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? City { get; set; }

        public virtual Cart Cart { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();

        public User()
        {
        }

        public User(Guid id, string username, string password, string email, string? phoneNumber = null,string? city = null)
        {
            Id = id;
            Username = username;
            Password = password;
            Email = email;
            PhoneNumber = phoneNumber;
            City = city;
        }
    }
}
