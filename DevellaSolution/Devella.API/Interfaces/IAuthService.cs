namespace Devella.API.Interfaces
{
    public interface IAuthService
    {
        List<string> GetUserRolesFromToken(string token);
    }
}
