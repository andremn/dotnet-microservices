namespace Orders.Application.Dtos;

public record LoggedUserDto(string Id, string FirstName, string LastName, string Email, string Authorization)
{
    public static readonly LoggedUserDto Empty = new(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
}
