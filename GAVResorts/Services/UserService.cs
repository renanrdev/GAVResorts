using GAVResorts.Models;
using GAVResorts.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GAVResorts.Services
{
    public class UserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public User Authenticate(string usuario, string senha)
        {
            if (string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(senha))
                return null;

            var user = _context.Users.SingleOrDefault(u => u.Usuario == usuario);

            if (user == null)
                return null;

            if (!VerifyPasswordHash(senha, user.Senha))
                return null;

            return user;
        }

        public User Create(User user)
        {
            if(_context.Users.Any(u => u.Usuario == user.Usuario))
            {
                throw new Exception("Usuário já existe");
            }

            user.Senha = CreatePasswordHash(user.Senha);

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        private string CreatePasswordHash(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPasswordHash(string password, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, storedHash);
        }
    }
}
