using BCrypt.Net;

namespace hamada.Services
{

    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string hashedPassword, string providedPassword);
    }

    public class PasswordService : IPasswordService
    {
        public string HashPassword(string password)
        {
             return BCrypt.Net.BCrypt.HashPassword(password);
             
        }

        public bool VerifyPassword(string hashedPassword, string providedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(providedPassword, hashedPassword);
        }
    }
}