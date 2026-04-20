namespace Demo.Core.DTOs;

/// <summary>
/// Auth Response.
/// </summary>
public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
}
