using Demo.Core.DTOs;
using Demo.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers;

/// <summary>
/// CRUD operations for policyholder records. All endpoints require authentication.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Returns all policyholders.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _userService.GetAllAsync();
        return Ok(users);
    }

    /// <summary>
    /// Returns a single policyholder by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Creates a new policyholder.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create([FromBody] RegisterDto dto)
    {
        var user = await _userService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Updates an existing policyholder.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UserUpdateDto dto)
    {
        var user = await _userService.UpdateAsync(id, dto);
        if (user == null) return NotFound();
        return Ok(user);
    }

    /// <summary>
    /// Deletes a policyholder by ID.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.DeleteAsync(id);
        if (!deleted) return NotFound();
        return NoContent();
    }

    /// <summary>
    /// Imports a policyholder from the RandomUser API.
    /// </summary>
    [HttpPost("import-random")]
    public async Task<ActionResult<UserDto>> ImportFromRandomUser()
    {
        var user = await _userService.ImportFromRandomUserAsync();
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Downloads a single policyholder's demographics as a CSV file.
    /// </summary>
    [HttpGet("document/{id}")]
    public async Task<IActionResult> DownloadDocument(int id)
    {
        var user = await _userService.GetByIdAsync(id);
        if (user == null) return NotFound();

        var csv = CsvHeader + Environment.NewLine + ToCsvRow(user);
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
        var fileName = $"{user.FirstName}_{user.LastName}_Demographics.csv";
        return File(bytes, "text/csv", fileName);
    }

    /// <summary>
    /// Exports all policyholder records as a CSV file.
    /// </summary>
    [HttpGet("export")]
    public async Task<IActionResult> ExportAll()
    {
        var users = await _userService.GetAllAsync();

        var lines = new List<string> { CsvHeader };
        foreach (var u in users)
            lines.Add(ToCsvRow(u));

        var csv = string.Join(Environment.NewLine, lines);
        var bytes = System.Text.Encoding.UTF8.GetBytes(csv);
        return File(bytes, "text/csv", "PolicyholderRecords.csv");
    }

    private const string CsvHeader =
        "First Name,Last Name,Email,Date of Birth,Phone,Street,City,State,Zip Code,Country,Created At";

    private static string ToCsvRow(UserDto u) => string.Join(",",
        $"\"{u.FirstName}\"",
        $"\"{u.LastName}\"",
        $"\"{u.Email}\"",
        $"\"{u.DateOfBirth?.ToString("yyyy-MM-dd") ?? ""}\"",
        $"\"{u.Phone ?? ""}\"",
        $"\"{u.Street ?? ""}\"",
        $"\"{u.City ?? ""}\"",
        $"\"{u.State ?? ""}\"",
        $"\"{u.ZipCode ?? ""}\"",
        $"\"{u.Country ?? ""}\"",
        $"\"{u.CreatedAt:yyyy-MM-dd HH:mm:ss}\"");
}
