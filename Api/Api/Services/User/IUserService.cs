namespace Api.Services;

public interface IUserService
{
    Task<Models.User.User> GetUser(string username);
}