
using Devella.DataAccessLayer.DTOs.UserAccess;
using Devella.DataAccessLayer.Models;

namespace Devella.DataAccessLayer.Mappers.UserAuth;
public static class UserMappers
{
    public static User ToUser(this RegisterDTO dto)
    {
        return new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            Surname = dto.Surname,
        };
    }
}
