using MediatR;

namespace Project1.Application.ApplicationUsers.Queries.GetApplicationUser
{
    public class GetApplicationUserCommand : IRequest<GetApplicationUserResponse>
    {
        public int Id { get; set; }

        public GetApplicationUserCommand(int id)
        {
            Id = id;
        }
    }
}