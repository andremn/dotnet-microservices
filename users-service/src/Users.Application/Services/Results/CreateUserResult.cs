using Users.Domain.Models;

namespace Users.Application.Services.Results;

public record CreateUserResult(bool Success, User? User, Dictionary<string, string> Errors);
