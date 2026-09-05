using Backend.Data;
using Backend.Models;
using Backend.Models.Entities;
using Backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Backend.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly BuildHubDbContext _context;

        public UserRepository(BuildHubDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync() =>
            await _context.Users.AsNoTracking().ToListAsync();

        public async Task<User?> GetByIdAsync(int id) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == id);

        public async Task<User?> GetByEmailAsync(string email) =>
            await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> UpdateAsync(int id, User input)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return null;

            // Editable profile details.
            user.FullName = input.FullName;
            user.PhoneNumber = input.PhoneNumber;
            user.City = input.City;

            // Deliberately not updated here:
            //   Email        - unique identity, needs a change-with-verification flow
            //   PasswordHash - belongs to a change-password flow, never a profile edit
            //   Role         - changing it is privilege escalation; admin-only action
            //   CreatedAt    - a historical fact

            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user is null) return false;

            // Throws DbUpdateException while the user still has jobs or reviews:
            // those relations are Restrict, so history is never silently destroyed.
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
