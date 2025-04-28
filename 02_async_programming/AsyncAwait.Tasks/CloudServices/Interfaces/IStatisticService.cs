using System.Threading;
using System.Threading.Tasks;

namespace CloudServices.Interfaces;

public interface IStatisticService
{
    Task RegisterVisitAsync(string url, CancellationToken token);

    Task<long> GetVisitsCountAsync(string url, CancellationToken token);
}
