using Project1.Models;

namespace Project1.Persistance
{
    public interface IAbsentRepository
    {
        public Task<Absent> CreateAbsentAsync(Absent absent, CancellationToken cancellationToken);
        public Task<Absent> UpdateAbsentAsync(Absent absent, CancellationToken cancellationToken);
        public Task<Absent> DeleteAbsentAsync(Guid id, CancellationToken cancellationToken);
        public Task<Absent> GetAbsentAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<Absent>> GetAllAbsentAsync(CancellationToken cancellationToken);
        public Task<List<Absent>> GetAbsentByIdAsync(int userId, CancellationToken cancellationToken);



    }
}

