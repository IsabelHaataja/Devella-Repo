using Devella.API.Interfaces;
using DevellaLib.DTOs.UserAccess;
using DevellaLib.Enums;
using DevellaLib.Mappers.Developer;
using DevellaLib.Helpers;
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

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    string loweredSearch = searchTerm.ToLowerInvariant();

                    // Map user input to matching enum values for each enum type
                    var matchedLanguages = Enum.GetValues(typeof(ProgrammingLanguage))
                        .Cast<ProgrammingLanguage>()
                        .Where(lang => EnumHelper.GetEnumDisplayName(lang).ToLowerInvariant().Contains(loweredSearch)
                                    || lang.ToString().ToLowerInvariant().Contains(loweredSearch))
                        .ToList();

                    var matchedAreas = Enum.GetValues(typeof(CompetenceArea))
                        .Cast<CompetenceArea>()
                        .Where(area => EnumHelper.GetEnumDisplayName(area).ToLowerInvariant().Contains(loweredSearch)
                                    || area.ToString().ToLowerInvariant().Contains(loweredSearch))
                        .ToList();

                    var matchedQualifications = Enum.GetValues(typeof(Qualification))
                        .Cast<Qualification>()
                        .Where(q => EnumHelper.GetEnumDisplayName(q).ToLowerInvariant().Contains(loweredSearch)
                                 || q.ToString().ToLowerInvariant().Contains(loweredSearch))
                        .ToList();

                    query = query.Where(d =>
                        d.User.FirstName.ToLower().Contains(loweredSearch) ||
                        d.User.Surname.ToLower().Contains(loweredSearch) ||
                        (d.Competence != null && (
                            d.Competence.ProgrammingLanguages.Any(pl => matchedLanguages.Contains(pl)) ||
                            d.Competence.CompetenceAreas.Any(ca => matchedAreas.Contains(ca)) ||
                            d.Competence.Qualifications.Any(q => matchedQualifications.Contains(q))
                        ))
                    );
                }

                // Apply sorting
                query = sortOption switch
                {
                    // Sort by Created date ascending
                    "Äldsta först" => query.OrderBy(c => c.User.Created),

                    // Sort by Created date descending
                    "Nyaste först" => query.OrderByDescending(c => c.User.Created),

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
