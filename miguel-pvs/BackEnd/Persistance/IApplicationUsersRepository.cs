using Microsoft.AspNetCore.Mvc;
using Project1.Models;
using Microsoft.AspNetCore.Http;
using Project1.Data;
using Project1.Persistance;

namespace Project1.Persistance
{
    public interface IApplicationUsersRepository
    {
        public Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser createdUser, CancellationToken cancellationToken);
        public Task<ApplicationUser> GetApplicationUserAsync(int id, CancellationToken cancellationToken);
        public Task<ApplicationUser> GetApplicationUserAsync(int id, bool track, CancellationToken cancellationToken);
        public Task<ApplicationUser> UpdateApplicationUserAsync(ApplicationUser updatedUser, CancellationToken cancellationToken);
        public Task<ApplicationUser> DeleteApplicationUserAsync(int applicationUser, CancellationToken cancellationToken);


        public Task<ApplicationUser> CheckInApplicationUserAsync(int applicationUser, CancellationToken cancellationToken);
        public Task<ApplicationUser> CheckOutApplicationUserAsync(int applicationUser, CancellationToken cancellationToken);

        public Task<UsersEachLocation> GetLocationAsync(int officeLocation, CancellationToken cancellationToken);

        public Task<List<ApplicationUser>> GetAllApplicationUserAsync(CancellationToken cancellationToken);
        
        public Task<List<UsersEachLocation>> GetAllLocationsAsync(CancellationToken cancellationToken);
        
    }
}
