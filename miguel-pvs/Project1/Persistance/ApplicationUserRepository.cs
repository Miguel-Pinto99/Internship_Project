using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Models;

namespace Project1.Persistance
{
    public class ApplicationUserRepository : IApplicationUsersRepository
    {
        private readonly AppDbContext _context;
        public ApplicationUserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> CreateApplicationUserAsync(ApplicationUser user, CancellationToken cancellationToken)
        {

            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            try
            {
                await _context.Users.AddAsync(user, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                user = null;
            }
            return user;
        }

        public async Task<ApplicationUser> UpdateApplicationUserAsync(ApplicationUser user, CancellationToken cancellationToken)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            var userInDb = await _context.Users.Include(x => x.WorkPatterns).ThenInclude(x => x.Parts).FirstOrDefaultAsync(x => x.Id == user.Id, cancellationToken);

            if (userInDb != null)
            {
                userInDb.FirstName = user.FirstName;
                userInDb.OfficeLocation = user.OfficeLocation;
                userInDb.CheckedIn = user.CheckedIn;
                await _context.SaveChangesAsync();
            }
            return userInDb;
        }

        public Task<ApplicationUser> GetApplicationUserAsync(int id, CancellationToken cancellationToken) => GetApplicationUserAsync(id, true, cancellationToken);
        public async Task<ApplicationUser> GetApplicationUserAsync(int id, bool track, CancellationToken cancellationToken)
        {
            ApplicationUser user = new ApplicationUser();

            IQueryable<ApplicationUser> query =  _context.Users.Include(x => x.WorkPatterns).ThenInclude(x => x.Parts);
            if (!track)
            {
                query = query.AsNoTracking();
            }
            user = await  query.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return user;

        }

        public async Task<List<ApplicationUser>> GetAllApplicationUserAsync(CancellationToken cancellationToken)
        {

            var result = await _context.Users.ToListAsync(cancellationToken);
            return result;

        }

        public async Task<ApplicationUser> DeleteApplicationUserAsync(int id, CancellationToken cancellationToken)
        {
            var result = await _context.Users.Include(x => x.WorkPatterns).ThenInclude(x => x.Parts).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            try
            {
                _context.Users.Remove(result);
                await _context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                result = null;
            }

            return result;
        }

        public async Task<ApplicationUser> CheckInApplicationUserAsync(int id, CancellationToken cancellationToken)
        {
            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            try
            {
                userInDb.CheckedIn = true;
                await _context.SaveChangesAsync();
                return userInDb;
            }
            catch
            {
                userInDb = null;
                return userInDb;
            }

        }
        public async Task<ApplicationUser> CheckOutApplicationUserAsync(int id, CancellationToken cancellationToken)

        {

            var userInDb = await _context.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            try
            {
                userInDb.CheckedIn = false;
                await _context.SaveChangesAsync();
                return userInDb;
            }
            catch
            {
                userInDb = null;
                return userInDb;
            }

        }
        public async Task<List<UsersEachLocation>> GetAllLocationsAsync(CancellationToken cancellationToken)

        {
            List<UsersEachLocation> usersOffice = await _context.Users
                   .GroupBy(p => p.OfficeLocation)
                   .Select(g => new UsersEachLocation(g.Key, g.Select(x => x.Id).ToList()))
                   .ToListAsync(cancellationToken);
            return usersOffice;
        }

        public async Task<UsersEachLocation> GetLocationAsync(int officeLocation, CancellationToken cancellationToken)
        {
            var user = await _context.Users.Where(x => x.OfficeLocation == officeLocation).ToListAsync(cancellationToken);
        
            UsersEachLocation userOffice = user
                   .GroupBy(p => p.OfficeLocation)
                   .Select(g => new UsersEachLocation(g.Key, g.Select(x => x.Id).ToList()))
                   .FirstOrDefault();

            return userOffice;
        }
    }
}
