using DevellaLib.DTOs.UserAccess;
using DevellaLib.Models;

namespace DevellaLib.Mappers.UserAuth;

public static class UserMapper
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