using MediatR;

namespace Project1.Application.ApplicationUsers.Queries.EditApplicationUser
{
    public class EditApplicationUserCommandBody
    {
        public string FirstName { get; set; }
        public int OfficeLocation { get; set; }
    }
}