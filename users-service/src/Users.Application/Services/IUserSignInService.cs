namespace Users.Application.Services;

public interface IUserSignInService
{
    Task<bool> SignInAsync(string email, string password);
}