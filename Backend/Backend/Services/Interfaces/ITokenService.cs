using Backend.Models.Dtos;
using Backend.Models.Entities;

namespace Backend.Services.Interfaces
{
    public interface ITokenService
    {
        /// <summary>
        /// Issues a signed bearer token carrying the user's id and role, so later
        /// requests no longer have to say who they are.
        /// </summary>
        AuthResponse CreateToken(User user);
    }
}
