using MediatR;

namespace Project1.Application.ApplicationUsers.Commands.CreateApplicationUser
{
    public class CreateApplicationUserCommandBody
    {
        public string FirstName { get; set; }
        public int OfficeLocation { get; set; }
    }
}