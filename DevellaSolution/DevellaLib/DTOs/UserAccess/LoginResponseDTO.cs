
namespace DevellaLib.DTOs.UserAccess;

public class LoginResponseDTO
{
    public string Token { get; set; }
    public long TokenExpired { get; set; }
}
