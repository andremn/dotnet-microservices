namespace Products.Controllers.Users;

public record CreateUserRequest(string FirstName, string LastName, string Email, string Password);
