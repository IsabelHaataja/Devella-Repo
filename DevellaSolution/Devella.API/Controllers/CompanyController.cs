using Devella.API.Interfaces;
using Devella.API.Repositories;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Mappers.Developer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Devella.API.Controllers
{
    [ApiController]
    [Route("api/company")]
    [Authorize]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepo;

        public CompanyController(ICompanyRepository companyRepo)
        {
            _companyRepo = companyRepo;
        }

        //[Authorize(Roles = "Client")]
        [HttpPost("save-developer/{developerId}")]
        public async Task<IActionResult> SaveDeveloper(int developerId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            try
            {
                await _companyRepo.SaveDeveloperToListAsync(userId, developerId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error saving developer: {ex.Message}");
            }
        }

        [Authorize(Roles = "Client")]
        [HttpGet("saved-developers")]
        public async Task<ActionResult<List<DeveloperProfileDTO>>> GetSavedDevelopers()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var savedDevs = await _companyRepo.GetSavedDevelopersAsync(userId); // or service
            var result = savedDevs.Select(d => DeveloperMapper.ToDto(d)).ToList();

            return Ok(result);
        }
    }
}
