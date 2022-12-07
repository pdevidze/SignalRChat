using Microsoft.AspNetCore.SignalR;

public interface IWorker
{
    Task DoWork(CancellationToken cancellationToken);
}

public class MyWorker : IWorker
{
    private readonly ILogger<MyWorker> logger;
    private IHubContext<ChatHub> HubContext { get; set; }
    private int number = 0;

    public MyWorker(ILogger<MyWorker> logger, IHubContext<ChatHub> hubcontext)
    {
        this.logger = logger;
        this.HubContext = hubcontext;
    }

    public async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Interlocked.Increment(ref number);
            logger.LogInformation($"Worker printing number: {number}");
            await this.HubContext.Clients.All.SendAsync("WorkerPrinted", number);
            await Task.Delay(1000 * 5);
        }
    }
}
