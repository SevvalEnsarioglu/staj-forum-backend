using staj_forum_backend.DTOs.Auth;
using staj_forum_backend.Models;

namespace staj_forum_backend.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto request);
    Task<AuthResponseDto> LoginAsync(LoginDto request);
    Task<User?> GetUserByIdAsync(int id);
}
