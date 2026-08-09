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

        public UserResponseDto? GetById(int id)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (user == null) { return null; }

            UserResponseDto response = new UserResponseDto()
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

        public UserResponseDto? GetByEmail(string email)
        {
            User user = _context.Users.FirstOrDefault(u => u.Email == email);

            UserResponseDto response = new UserResponseDto()
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

        public void Delete(int id)
        {
            User user = _context.Users.FirstOrDefault(u => u.UserId == id);
            _context.Users.Remove(user);
            _context.SaveChanges();
        }
    }
}
