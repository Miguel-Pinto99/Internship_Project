using BlazorApp1.Model;

namespace Project1.Infrastructure
{
    public interface IUnsService
    {
        public Task<List<ApplicationUser>> SubscribeBrokerAsync(CancellationToken cancellationToken);
    }
}
