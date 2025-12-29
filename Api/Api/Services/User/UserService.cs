using Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Services.User;

public class UserService(LiverpoolDbContext dbContext) : IUserService
{
    public async Task<Models.User.User> GetUser(string username)
    {
        var user = await dbContext.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
        
        if (user == null)
            throw new ApplicationException($"User with username {username} does not exist");
        
        var userModel = user.ToDto();
        return userModel;
    }
}