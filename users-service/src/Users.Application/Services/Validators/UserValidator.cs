using FluentValidation;
using Users.Domain.Models;

namespace Users.Application.Services.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x =>  x.FirstName).NotEmpty();
        RuleFor(x =>  x.LastName).NotEmpty();
        RuleFor(x =>  x.Email).EmailAddress();
    }
}