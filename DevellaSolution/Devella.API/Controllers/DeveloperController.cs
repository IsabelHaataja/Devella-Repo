using Devella.API.Interfaces;
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Mappers.Developer;
using DevellaLib.Services.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet("profiles")]
        public async Task<ActionResult<IEnumerable<DeveloperProfileDTO>>> GetAllProfiles()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            try
            {
                var devProfiles = await _repo.GetDeveloperProfilesAsync();
                if (devProfiles == null)
                {
                    return NotFound("Developer profile not found.");
                }

                var dtos = new List<DeveloperProfileDTO>();

                foreach (var profile in devProfiles)
                {
                    dtos.Add(DeveloperMapper.ToDto(profile));
                }

                return Ok(dtos);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error retrieving profiles: {ex.Message}");
            }
        }

        [HttpGet("paged")]
        [Authorize]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber, [FromQuery] int pageSize,
    [FromQuery] string? searchTerm, [FromQuery] string? sortOption)
        {
            // Validate pageNumber
            if (pageNumber <= 0)
            {
                return BadRequest("Page number must be greater than zero.");
            }

            try
            {
                var query = _repo.GetAllQueryable();

                // Apply search filter
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    var lowered = searchTerm.ToLower();
                    query = query.Where(c =>
                    c.User.FirstName.ToLower().Contains(lowered) ||
                    c.User.Surname.ToLower().Contains(lowered) ||
                        (!string.IsNullOrEmpty(c.Description) && c.Description.ToLower().Contains(lowered)) ||
                        (c.Competence != null && (
                            c.Competence.ProgrammingLanguages.Any(lang => lang.ToString().ToLower().Contains(lowered)) ||
                            c.Competence.Qualifications.Any(q => q.ToString().ToLower().Contains(lowered)) ||
                            c.Competence.CompetenceAreas.Any(a => a.ToString().ToLower().Contains(lowered)) ||
                            c.Competence.CompetenceLevel.Any(lvl => lvl.ToString().ToLower().Contains(lowered))
                        ))
                    );
                }

                // Apply sorting
                query = sortOption switch
                {
                    "Anställningsform" => query.OrderBy(c => c.WantedPosition),
                    "Erfarenhet" => query.OrderByDescending(c => c.Experience),
                    _ => query.OrderBy(c => c.Id)
                };

                var totalItems = await query.CountAsync();
                var items = await query
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var profileDTOs = items.Select(DeveloperMapper.ToDto).ToList();

                var pagedResult = new PagedResult<DeveloperProfileDTO>
                {
                    CurrentPage = pageNumber,
                    PageSize = pageSize,
                    RowCount = totalItems,
                    PageCount = (int)Math.Ceiling((double)totalItems / pageSize),
                    Results = profileDTOs
                };

                return Ok(pagedResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetPaged profiles exception: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
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
