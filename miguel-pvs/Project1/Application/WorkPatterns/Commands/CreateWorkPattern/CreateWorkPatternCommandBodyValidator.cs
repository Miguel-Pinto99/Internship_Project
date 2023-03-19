using FluentValidation;
using Project1.Models;

namespace Project1.Application.WorkPatterns.Commands.CreateWorkPattern;

public class CreateWorkPatternCommandBodyValidator : AbstractValidator<CreateWorkPatternCommandBody>
{
    public CreateWorkPatternCommandBodyValidator()
    {

        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).GreaterThan(x => x.StartDate);
        RuleFor(x => x.Parts).NotEmpty();
        RuleForEach(x => x.Parts)
            .SetValidator(new WorkPatternPartValidator());
    }   
}
public class WorkPatternPartValidator : AbstractValidator<WorkPatternPart>
{
    public WorkPatternPartValidator()
    {
        RuleFor(x => x.StartTime).GreaterThanOrEqualTo(TimeSpan.Zero);
        RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime);
        RuleFor(x => x.EndTime).LessThan(TimeSpan.FromDays(1));
        RuleFor(x => x.Day).NotEmpty();
    }
}