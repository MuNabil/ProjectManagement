using Domain.DTOs;

namespace Domain.Interfaces
{
    public interface IAccountRepository
    {
        Task<AuthenticationDto> Register(RegisterDto model);
        Task<AuthenticationDto> Login(LoginDto model);
        Task Logout();
        Task<bool> ChangePassword(UserChangePasswordDto model);
        Task<AuthenticationDto> ResetPassword(LoginDto model);
    }
}
