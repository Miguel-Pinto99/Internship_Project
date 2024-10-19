using FluentValidation;
using Project1.Application.Absent.Commands.DeleteAbsent;

namespace Project1.Application.Absent.Commands.DeleteAbsent
{
    public class DeleteAbsentCommandValidator : AbstractValidator<DeleteAbsentCommand>
    {
        public DeleteAbsentCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}