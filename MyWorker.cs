using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using System.Threading;

namespace SignalRChat
{
  public interface IWorker
  {
    Task DoWork(CancellationToken cancellationToken);
  }

  public class MyWorker : IWorker
  {
    private readonly ILogger<MyWorker> logger;
    private IHubContext<ChatHub> HubContext { get; set; }
    private int paymentId = 0;

    public MyWorker(ILogger<MyWorker> logger, IHubContext<ChatHub> hubcontext)
    {
      this.logger = logger;
      this.HubContext = hubcontext;
    }

    public async Task DoWork(CancellationToken cancellationToken)
    {
      while (!cancellationToken.IsCancellationRequested)
      {
        Interlocked.Increment(ref paymentId);
        logger.LogInformation($"Worker job finished. paymentId: {paymentId}");
        await this.HubContext.Clients.All.SendAsync("RecieveJobFinished", paymentId);
        await Task.Delay(1000 * 5);
      }
    }
  }
}
