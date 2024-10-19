using FluentValidation;
using Project1.Application.ApplicationUsers.Commands.CreateApplicationUser;

namespace Project1.Application.ApplicationUser.Commands.CreateApplicationUser;

public class CreateApplicationUserCommandBodyValidator : AbstractValidator<CreateApplicationUserCommandBody>
{
    public CreateApplicationUserCommandBodyValidator()
    {

        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.OfficeLocation).GreaterThan(0);
    }
}
