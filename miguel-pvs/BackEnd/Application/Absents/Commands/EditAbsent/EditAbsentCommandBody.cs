using MediatR;
using Project1.Models;

namespace Project1.Application.ApplicationUsers.Queries.EditWorkPatterm
{
    public class EditAbsentCommandBody
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string Description { get; set; }
    }
}