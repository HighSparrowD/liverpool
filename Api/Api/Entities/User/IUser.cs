namespace Api.Entities.User;

public interface IUser
{
    void HashPassword(string password);
    
    bool LogIn(string password);
}