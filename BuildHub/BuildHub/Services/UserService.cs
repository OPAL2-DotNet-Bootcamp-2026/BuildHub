using BuildHub.DTOs;
using BuildHub.Models;
using BuildHub.Repos;

namespace BuildHub.Services
{
    public class UserService
    {
        private UserRepo _userRepo;

        public UserService(UserRepo userRepo)
        {
            _userRepo = userRepo;
        }

        public List<User> GetAllUsers()
        {
            return _userRepo.GetAll();
        }

        public User GetUserById(int id)
        {
            return _userRepo.GetById(id);
        }

        public int AddUser(User user)
        {
            _userRepo.Add(user);
            return user.UserId;
        }

        // this is example on email update
        public UserResponseDTO UpdateUser(int id, UserUpdateDTO dto)
        {
            User user = _userRepo.GetById(id);
            if (user == null)
            {
                return null;
            }
            
            user.Email = dto.Email;
            _userRepo.Update();
            
            UserResponseDTO response = new UserResponseDTO();
            response.UserId = user.UserId;
            response.FullName = user.FullName;
            response.Email = user.Email;
            
            return response;
        }

        public bool DeleteUser(int id)
        {
            User user = _userRepo.GetById(id);
            if (user == null)
            {
                return false;
            }
            
            _userRepo.Delete(user);
            return true;
        }
    }
}
