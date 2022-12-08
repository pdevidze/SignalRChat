using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace SignalRChat
{
  public class MyBackgroundService : BackgroundService
  {
    private readonly IWorker worker;

    public MyBackgroundService(IWorker worker)
    {
      this.worker = worker;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
      await worker.DoWork(stoppingToken);
    }
  }
}
