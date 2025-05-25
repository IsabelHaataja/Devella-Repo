using Devella.API.Interfaces;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Devella.DataAccessLayer.Mappers.UserAuth;
using Devella.API.Repositories;
using Devella.DataAccessLayer.Enums;

namespace Devella.API.Controllers;
    [ApiController]
    [Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthRepository _authRepo;

    public AuthController(UserManager<User> userManager, IAuthRepository authRepo)
    {
        _userManager = userManager;
        _authRepo = authRepo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] RegisterDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var user = model.ToUser();
            var result = await _authRepo.CreateUserAsync(user, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            await _authRepo.AddUserToRoleAsync(user, model.Role.ToString());

            if (model.Role == Role.Developer)
            {
                await _authRepo.CreateDeveloperProfileAsync(user.Id);
            }
            else if (model.Role == Role.Client)
            {
                await _authRepo.CreateClientProfileAsync(user.Id);
            }

            return Ok(new { Message = "User registered successfully!" });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during registration: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine("Invalid login request");
            return BadRequest(ModelState);
        }

        try
        {
            var user = await _authRepo.ValidateUserAsync(model);
            if (user == null)
                return Unauthorized("Invalid email or password");

            var token = await _authRepo.GenerateJwtToken(user);

            return Ok(new LoginResponseDTO
            {
                Token = token,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(3).ToUnixTimeSeconds(),
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
}
