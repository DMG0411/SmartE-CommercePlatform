using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Exceptions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext context;

        public UserRepository(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<Guid> CreateUser(User user)
        {
            user.Password = EncryptPassword(user.Password, GenerateSalt());

            var existingUser = await context.Users.FirstOrDefaultAsync(x => x.Email == user.Email);

            if (existingUser != null)
            {
                throw new ConflictException(ExceptionsResource.Conflict);
            }

            await context.Users.AddAsync(user);
            await context.Carts.AddAsync(new Cart { User = user });
            await context.SaveChangesAsync();
            return user.Id;
        }

        public async Task<User> LoginUser(User user)
        {
            var userResult = await context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userResult == null || !VerifyPassword(user.Password, userResult.Password))
            {
                throw new ResourceNotFoundException(ExceptionsResource.NoResourceFound);
            }
            return userResult;
        }

        public async Task DeleteUser(Guid id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                throw new ResourceNotFoundException(ExceptionsResource.NoResourceFound);
            }
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }

        public Task<User> GetUserById(Guid id)
        {
            var user = context.Users.Find(id);
            if (user == null)
            {
                throw new ResourceNotFoundException(ExceptionsResource.NoResourceFound);
            }
            return Task.FromResult(user);
        }

        public async Task UpdateUser(User user)
        {
            var userEntity = context.Users.Find(user.Id);
            if (userEntity == null)
            {
                throw new ResourceNotFoundException(ExceptionsResource.NoResourceFound);
            }
            userEntity.Username = user.Username;
            userEntity.PhoneNumber = user.PhoneNumber;
            userEntity.City = user.City;

            context.Users.Update(userEntity);
            await context.SaveChangesAsync();
        }

        private static string EncryptPassword(string password, byte[] salt)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
                byte[] saltedPassword = new byte[passwordBytes.Length + salt.Length];

                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, 0, passwordBytes.Length);
                Buffer.BlockCopy(salt, 0, saltedPassword, passwordBytes.Length, salt.Length);

                byte[] hashedBytes = sha256.ComputeHash(saltedPassword);

                byte[] hashedPasswordWithSalt = new byte[hashedBytes.Length + salt.Length];
                Buffer.BlockCopy(salt, 0, hashedPasswordWithSalt, 0, salt.Length);
                Buffer.BlockCopy(hashedBytes, 0, hashedPasswordWithSalt, salt.Length, hashedBytes.Length);

                return Convert.ToBase64String(hashedPasswordWithSalt);
            }
        }

        private static byte[] GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] salt = new byte[16];
                rng.GetBytes(salt);
                return salt;
            }
        }

        private bool VerifyPassword(string password, string hashedPassword)
        {
            byte[] hashedPasswordBytes = Convert.FromBase64String(hashedPassword);
            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashedPasswordBytes, 0, salt, 0, 16);

            string hashedPasswordToVerify = EncryptPassword(password, salt);

            return hashedPassword == hashedPasswordToVerify;
        }
    }
}
