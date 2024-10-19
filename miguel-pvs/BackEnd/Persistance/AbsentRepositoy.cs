using Project1.Data;
using Project1.Models;
using Microsoft.EntityFrameworkCore;

namespace Project1.Persistance
{
    public class AbsentRepository : IAbsentRepository
    {
        private readonly AppDbContext _context;
        public AbsentRepository(AppDbContext context)
        {

            _context = context;
        }
        public async Task<Absent> CreateAbsentAsync(Absent absent, CancellationToken cancellationToken)
        {
            if (absent is null)
            {
                throw new ArgumentNullException(nameof(absent));
            }

            try
            {
                absent.Id = Guid.NewGuid();

                await _context.Absent.AddAsync(absent, cancellationToken);
                await _context.SaveChangesAsync();

            }
            catch
            {
                absent = null;
            }
            return absent;
        }
        public async Task<Absent> UpdateAbsentAsync(Absent absent, CancellationToken cancellationToken)
        {
            if (absent is null)
            {
                throw new ArgumentNullException(nameof(absent));
            }

            var result = await _context.Absent.FirstOrDefaultAsync(x => x.Id == absent.Id, cancellationToken);
            if (result != null)
            {
                result.StartDate = absent.StartDate;
                result.EndDate = absent.EndDate;
            }
            return result;
        }
        public async Task<Absent> GetAbsentAsync(Guid id, CancellationToken cancellationToken)
        {

            var result = await _context.Absent.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return result;

        }
        public async Task<List<Absent>> GetAllAbsentAsync(CancellationToken cancellationToken)
        {

            var result = await _context.Absent.ToListAsync(cancellationToken);
            return result;

        }

        public async Task<List<Absent>> GetAbsentByIdAsync(int userId, CancellationToken cancellationToken)
        {

            var result = await _context.Absent.Where(x => x.UserId == userId).ToListAsync();
            return result;

        }
        public async Task<Absent> DeleteAbsentAsync(Guid id, CancellationToken cancellationToken)
        {
            var result = await _context.Absent.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            try
            {
                _context.Absent.Remove(result);
                _context.SaveChanges();
            }
            catch
            {

            }
            return result;
        }
    }

}

