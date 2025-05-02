using Devella.API.Interfaces;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        if (model.Password != model.ConfirmPassword)
        {
            return BadRequest(new { Message = "Passwords do not match" });
        }

        var user = new User 
        { 
            UserName = model.Email, 
            Email = model.Email, 
            FirstName = model.FullName,
            Surname = model.Surname,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new { Message = "User registered successfully!" });
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
