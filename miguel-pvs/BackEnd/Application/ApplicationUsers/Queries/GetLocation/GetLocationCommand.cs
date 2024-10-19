using MediatR;

namespace Project1.Application.ApplicationUsers.Queries.GetLocation
{
    public class GetLocationCommand : IRequest<GetLocationResponse>
    {
        public int OfficeLocation { get; set; }
        public GetLocationCommand(int officeLocation)
        {
            OfficeLocation = officeLocation;
        }
    }
}