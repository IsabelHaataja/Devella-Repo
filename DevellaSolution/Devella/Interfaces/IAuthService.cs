namespace Devella.Interfaces;

public interface IAuthService
{
    List<string> GetUserRolesFromToken(string token);
}
