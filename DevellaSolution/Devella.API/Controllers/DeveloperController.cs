using Devella.API.Interfaces;
using Devella.API.Repositories;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Mappers.Developer;
using Devella.DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Devella.API.Controllers
{
    [ApiController]
    [Route("api/developer")]
    [Authorize]
    public class DeveloperController : ControllerBase
    {
        private readonly IDeveloperRepository _repo;

        public DeveloperController(IDeveloperRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<DeveloperProfileDTO>> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var devProfile = await _repo.GetDeveloperProfileAsync(userId);
            if (devProfile == null)
            {
                return NotFound("Developer profile not found.");
            }

            var dto = DeveloperMapper.ToDto(devProfile);
            return Ok(dto);
        }

        [HttpPatch("update")]
        public async Task<ActionResult> UpdateProfile([FromBody] UpdateDevProfileDTO updateDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                Console.WriteLine(JsonConvert.SerializeObject(updateDto));


                var updatedProfile = await _repo.UpdateDeveloperProfileAsync(userId, updateDto);
                if (updatedProfile == null)
                    return NotFound();

                var resultDto = DeveloperMapper.ToDto(updatedProfile);
                return Ok(resultDto);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error updating profile: {ex.Message}");
            }
        }
    }
}
