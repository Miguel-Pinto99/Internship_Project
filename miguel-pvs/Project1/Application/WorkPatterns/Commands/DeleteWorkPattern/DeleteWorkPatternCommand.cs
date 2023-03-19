using MediatR;
using Project1.Application.WorkPatterns.Commands.CreateWorkPattern;

namespace Project1.Application.WorkPatterns.Commands.DeleteWorkPattern
{
    public class DeleteWorkPatternCommand : IRequest<DeleteWorkPatternResponse>
    {
        public Guid Id { get; set; }

        public DeleteWorkPatternCommand(Guid id)
        {
            Id = id;
        }
    }
}