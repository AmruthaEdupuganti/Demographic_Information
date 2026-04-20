using Demo.Core.Entities;

namespace Demo.Core.DTOs;

/// <summary>
/// Data transfer object representing a policyholder.
/// </summary>
public class UserDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public string? Phone { get; set; }
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Maps a <see cref="User"/> entity to a <see cref="UserDto"/>.
    /// </summary>
    public static UserDto FromEntity(User user) => new()
    {
        Id = user.Id,
        FirstName = user.FirstName,
        LastName = user.LastName,
        Email = user.Email,
        DateOfBirth = user.DateOfBirth,
        Phone = user.Phone,
        Street = user.Street,
        City = user.City,
        State = user.State,
        ZipCode = user.ZipCode,
        Country = user.Country,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };
}
