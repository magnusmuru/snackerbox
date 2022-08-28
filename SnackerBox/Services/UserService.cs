using SnackerBox.Dto;
using SnackerBox.Services.Interfaces;

namespace SnackerBox.Services;

public class UserService : IUserService
{
    public UserDto? FindUserByUsername(string userName)
    {
        // This service is a static to return some default users
        // Real application would check repository for valid user object

        if (userName == "garyvee")
        {
            return new UserDto
            {
                Username = "garyvee",
                Password = "garyvee"
            };
        }

        if (userName == "homelander")
        {
            return new UserDto
            {
                Username = "homelander",
                Password = "homelander"
            };
        }

        return null;
    }
}