using Orders.Application.Dtos;

namespace Orders.Application.Common;

public class LoggedUserProvider : ILoggedUserProvider
{
    public LoggedUserDto LoggedUser { get; set; } = LoggedUserDto.Empty;
}
