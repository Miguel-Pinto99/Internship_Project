using Microsoft.AspNetCore.Mvc;
using Project1.Models;
using Microsoft.AspNetCore.Http;
using Project1.Data;
using Project1.Persistance;

namespace Project1.Persistance
{
    public interface IWorkPatternRepository
    {
        public Task<WorkPattern> CreateWorkPatternAsync(WorkPattern workPattern, CancellationToken cancellationToken);
        public Task<WorkPattern> UpdateWorkPatternAsync(WorkPattern workPattern, CancellationToken cancellationToken);
        public Task<WorkPattern> DeleteWorkPatternAsync(Guid id, CancellationToken cancellationToken);
        public Task<WorkPattern> GetWorkPatternAsync(Guid id, CancellationToken cancellationToken);
        public Task<List<WorkPattern>> GetAllWorkPatternsAsync(CancellationToken cancellationToken);


    }
}
