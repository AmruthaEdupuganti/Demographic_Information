using Demo.Core.DTOs;

namespace Demo.Core.Interfaces;

/// <summary>
/// Handles user authentication (registration and login).
/// </summary>
public interface IAuthService
{
    /// <summary>Registers a new user and returns a JWT token.</summary>
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);

    /// <summary>Authenticates a user and returns a JWT token.</summary>
    Task<AuthResponseDto> LoginAsync(LoginDto dto);
}
