using Api.Data;
using Api.Models.User;
using Microsoft.EntityFrameworkCore;
using ApplicationException = System.ApplicationException;

namespace Api.Services.User;

public class RegistrationService(LiverpoolDbContext dbContext, TimeProvider timeProvider) : IRegistrationService
{
    public async Task<Models.User.User> CreateUser(CreateUser model)
    {
        var existingUser = await GetExistingUser(model.Username);
        
        if (existingUser != null)
            throw new ApplicationException($"Username {model.Username} is already taken");
            
        var user = new Entities.User.User()
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            Username = model.Username,
            Description = model.Description,
            DateOfBirth = model.DateOfBirth,
            PasswordHash = string.Empty,
            PasswordSalt = string.Empty,
            ProfilePictureBase64 = string.Empty,
            RegisteredAt = timeProvider.GetUtcNow().UtcDateTime
        };
        
        user.HashPassword(model.Password);
        
        await dbContext.Users.AddAsync(user);
        await dbContext.SaveChangesAsync();
        return user.ToDto();
    }

    public async Task<Models.User.User> UpdateUser(UpdateUser model)
    {
        var existingUser = await GetExistingUser(model.Username);
        
        if (existingUser == null)
            throw new ApplicationException($"User {model.Username} does not exist.");
        
        existingUser.FirstName = model.FirstName;
        existingUser.LastName = model.LastName;
        existingUser.Username = model.Username;
        existingUser.Description = model.Description;
        existingUser.DateOfBirth = model.DateOfBirth;
        existingUser.ProfilePictureBase64 = model.PhotoBase64;
        existingUser.UpdatedAt = timeProvider.GetUtcNow().UtcDateTime;
        
        // Changed password if one is provided
        if (!string.IsNullOrEmpty(model.Password))
            existingUser.HashPassword(model.Password);
        
        await dbContext.SaveChangesAsync();
        return existingUser.ToDto();
    }

    public async Task<Models.User.User> LoginUser(LoginUser model)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(x => string.Equals(x.Username, model.Username));

        if (user == null)
            throw new NullReferenceException($"User {model.Username} not found");
        
        if (!user.LogIn(model.Password))
            throw new ApplicationException($"Invalid password for user {model.Username}");
        
        return user.ToDto();
    }
    
    private async Task<Entities.User.User?> GetExistingUser(string username) => 
        await dbContext.Users.FirstOrDefaultAsync(x => x.Username == username);
}