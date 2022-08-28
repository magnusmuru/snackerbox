using SnackerBox.Dto;

namespace SnackerBox.Services.Interfaces;

public interface IUserService
{
    UserDto? FindUserByUsername(string userName);
}