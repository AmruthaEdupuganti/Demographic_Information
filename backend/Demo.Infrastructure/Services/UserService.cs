using System.Net.Http.Json;
using System.Text.Json;
using Demo.Core.DTOs;
using Demo.Core.Entities;
using Demo.Core.Interfaces;
using Demo.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Demo.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _context;
    private readonly IHttpClientFactory _httpClientFactory;

    public UserService(AppDbContext context, IHttpClientFactory httpClientFactory)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return await _context.Users
            .OrderByDescending(u => u.CreatedAt)
            .Select(u => UserDto.FromEntity(u))
            .ToListAsync();
    }

    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? null : UserDto.FromEntity(user);
    }

    public async Task<UserDto> CreateAsync(RegisterDto dto)
    {
        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            DateOfBirth = dto.DateOfBirth,
            Phone = dto.Phone,
            Street = dto.Street,
            City = dto.City,
            State = dto.State,
            ZipCode = dto.ZipCode,
            Country = dto.Country,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return UserDto.FromEntity(user);
    }

    public async Task<UserDto?> UpdateAsync(int id, UserUpdateDto dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return null;

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.Email = dto.Email;
        user.DateOfBirth = dto.DateOfBirth;
        user.Phone = dto.Phone;
        user.Street = dto.Street;
        user.City = dto.City;
        user.State = dto.State;
        user.ZipCode = dto.ZipCode;
        user.Country = dto.Country;
        user.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return UserDto.FromEntity(user);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return false;

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<UserDto> ImportFromRandomUserAsync()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetFromJsonAsync<JsonElement>("https://randomuser.me/api/");

        var result = response.GetProperty("results")[0];
        var name = result.GetProperty("name");
        var location = result.GetProperty("location");
        var street = location.GetProperty("street");
        var dob = result.GetProperty("dob");

        var user = new User
        {
            FirstName = name.GetProperty("first").GetString() ?? string.Empty,
            LastName = name.GetProperty("last").GetString() ?? string.Empty,
            Email = result.GetProperty("email").GetString() ?? string.Empty,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("ImportedUser123!"),
            DateOfBirth = DateTime.Parse(dob.GetProperty("date").GetString()!),
            Phone = result.GetProperty("phone").GetString(),
            Street = $"{street.GetProperty("number").GetInt32()} {street.GetProperty("name").GetString()}",
            City = location.GetProperty("city").GetString(),
            State = location.GetProperty("state").GetString(),
            ZipCode = location.GetProperty("postcode").GetRawText().Trim('"'),
            Country = location.GetProperty("country").GetString(),
            CreatedAt = DateTime.UtcNow
        };

        // If email already exists, append a random suffix
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
        {
            var suffix = Guid.NewGuid().ToString("N")[..6];
            var parts = user.Email.Split('@');
            user.Email = $"{parts[0]}+{suffix}@{parts[1]}";
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return UserDto.FromEntity(user);
    }
}
