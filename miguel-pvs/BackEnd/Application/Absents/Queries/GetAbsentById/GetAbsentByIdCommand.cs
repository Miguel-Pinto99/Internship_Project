using MediatR;

namespace Project1.Application.Absent.Queries.GetAbsentByIdById
{
    public class GetAbsentByIdCommand : IRequest<GetAbsentByIdResponse>
    {
        public int UserId { get; set; }

        public GetAbsentByIdCommand(int userId)
        {
            UserId = userId;
        }
    }
}