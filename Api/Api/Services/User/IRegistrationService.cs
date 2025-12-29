using Api.Models.User;

namespace Api.Services.User;

public interface IRegistrationService
{
    Task<Models.User.User> CreateUser(CreateUser model);
    
    Task<Models.User.User> UpdateUser(UpdateUser model);
    
    Task<Models.User.User> LoginUser(LoginUser model);
}