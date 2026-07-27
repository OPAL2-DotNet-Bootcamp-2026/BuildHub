using BuildHub.DTOs;
using BuildHub.Models;

namespace BuildHub.Repos
{
    public class UserRepo
    {
        private ProjectContext _context;

        public UserRepo(ProjectContext context)
        {
            _context = context;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public UserResponseDTO? GetById(int id)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserId == id);

            UserResponseDTO response = new UserResponseDTO()
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                City = user.City,
                IsVerified = user.IsVerified,
                CreatedAt = user.CreatedAt,
            };

            return response;
        }

        public UserResponseDTO? GetByEmail(string email)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);

            UserResponseDTO response = new UserResponseDTO()
            {
                UserId = user.UserId,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                City = user.City,
                CreatedAt = user.CreatedAt,
            };

            return response;
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Update()
        {
            _context.SaveChanges();
        }

        public void Delete(User user)
        {
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
