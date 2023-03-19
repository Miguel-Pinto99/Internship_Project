using MediatR;

namespace Project1.Application.WorkPatterns.Queries.GetWorkPattern
{
    public class GetWorkPatternCommand : IRequest<GetWorkPatternResponse>
    {
        public Guid Id { get; set; }
        public GetWorkPatternCommand(Guid id)
        {
            Id = id;
        }
    }
}