using Demo.Core.DTOs;

namespace Demo.Core.Interfaces;

/// <summary>
/// CRUD and import operations for policyholder records.
/// </summary>
public interface IUserService
{
    /// <summary>Returns all policyholders.</summary>
    Task<IEnumerable<UserDto>> GetAllAsync();

    /// <summary>Returns a single policyholder by ID, or null if not found.</summary>
    Task<UserDto?> GetByIdAsync(int id);

    /// <summary>Creates a new policyholder.</summary>
    Task<UserDto> CreateAsync(RegisterDto dto);

    /// <summary>Updates an existing policyholder, or returns null if not found.</summary>
    Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto);

    /// <summary>Deletes a policyholder. Returns false if not found.</summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>Imports a policyholder from the RandomUser API.</summary>
    Task<UserDto> ImportFromRandomUserAsync();
}
