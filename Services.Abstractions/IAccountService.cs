using HelpersDTO.Authentication;
using HelpersDTO.Authentication.DTO.Models;

namespace Services.Abstractions
{
    public interface IAccountService
    {
        Task<RegistrationResponse?> RegisterAsync(RegisterDTO register);
        Task<LoginResponse?> LoginAsync(LoginDTO login);
    }
}
