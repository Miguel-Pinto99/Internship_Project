using Project1.Data;
using Project1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Client;
using MQTTnet;
using Newtonsoft.Json;
using System.Linq.Expressions;

namespace Project1.Persistance
{
    public class WorkPatternRepository : IWorkPatternRepository
    {
        private readonly AppDbContext _context;
        public WorkPatternRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<WorkPattern> CreateWorkPatternAsync(WorkPattern workPattern, CancellationToken cancellationToken)
        {
            if (workPattern is null)
            {
                throw new ArgumentNullException(nameof(workPattern));
            }

            try
            {
                workPattern.Id = Guid.NewGuid();

                foreach (var workPatternPart in workPattern.Parts)
                    workPatternPart.Id = Guid.NewGuid();

                await _context.WorkPattern.AddAsync(workPattern, cancellationToken);
                await _context.SaveChangesAsync();

            }
            catch
            {
                workPattern = null;
            }
            return workPattern;
        }
        public async Task<WorkPattern> UpdateWorkPatternAsync(WorkPattern workPattern, CancellationToken cancellationToken)
        {
            if (workPattern is null)
            {
                throw new ArgumentNullException(nameof(workPattern));
            }

            var result = await _context.WorkPattern.Include(x => x.Parts).FirstOrDefaultAsync(x => x.Id == workPattern.Id, cancellationToken);
            if (result != null)
            {
                result.StartDate = workPattern.StartDate;
                result.EndDate = workPattern.EndDate;
                result.Parts = workPattern.Parts;
            }
            return result;
        }
        public async Task<WorkPattern> GetWorkPatternAsync(Guid id, CancellationToken cancellationToken)
        {

            var result = await _context.WorkPattern.Include(x => x.Parts).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            return result;

        }
        public async Task<List<WorkPattern>> GetAllWorkPatternsAsync(CancellationToken cancellationToken)
        {

            var result = await _context.WorkPattern.Include(x => x.Parts).ToListAsync(cancellationToken);
            return result;

        }
        public async Task<WorkPattern> DeleteWorkPatternAsync(Guid id, CancellationToken cancellationToken)
        {         
            var result = await _context.WorkPattern.Include(x => x.Parts).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            try
            {
                _context.WorkPattern.Remove(result);
                _context.SaveChanges();
            }
            catch
            {

            }
            return result;
        }
    }

}



