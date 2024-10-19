using FluentValidation;

namespace Project1.Application.ApplicationUsers.Commands.DeleteApplicationUser
{
    public class DeleteApplicationUserCommandValidator : AbstractValidator<DeleteApplicationUserCommand>
    {
        public DeleteApplicationUserCommandValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0);
        }
    }
}