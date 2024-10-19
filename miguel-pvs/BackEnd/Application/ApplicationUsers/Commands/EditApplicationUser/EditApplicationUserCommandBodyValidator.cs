using FluentValidation;
using Project1.Application.ApplicationUsers.Queries.EditApplicationUser;

namespace Project1.Application.ApplicationUsers.Commands.EditApplicationUser;

public class EditApplicationUserCommandBodyValidator : AbstractValidator<EditApplicationUserCommandBody>
{
    public EditApplicationUserCommandBodyValidator()
    {

        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.OfficeLocation).GreaterThan(0);
    }
}
