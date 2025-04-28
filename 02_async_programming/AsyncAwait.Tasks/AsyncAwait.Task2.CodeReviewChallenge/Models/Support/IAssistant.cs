using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task2.CodeReviewChallenge.Models.Support;

public interface IAssistant
{
    Task<string> RequestAssistanceAsync(string requestInfo, CancellationToken token);
}
