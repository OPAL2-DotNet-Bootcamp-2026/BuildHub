using BuildHub.Repos;

namespace BuildHub.Services
{
    public class milestoneService
    {
        private milestoneRepo milestoneRepo;
        private AuthService authService;

        public UserService(UserRepo _userRepo, AuthService _authService)
        {
            userRepo = _userRepo;
            authService = _authService;
        }

    }
}
