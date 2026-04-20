using System.ComponentModel.DataAnnotations;

namespace Demo.Core.DTOs;

/// <summary>
/// User Update Info.
/// </summary>
public class UserUpdateDto
{
    [Required, MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(256)]
    public string Email { get; set; } = string.Empty;

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(20)]
    public string? Phone { get; set; }

    [MaxLength(256)]
    public string? Street { get; set; }

    [MaxLength(100)]
    public string? City { get; set; }

    [MaxLength(50)]
    public string? State { get; set; }

    [MaxLength(20)]
    public string? ZipCode { get; set; }

    [MaxLength(100)]
    public string? Country { get; set; }
}
