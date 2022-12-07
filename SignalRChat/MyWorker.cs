public interface IWorker
{
    Task DoWork(CancellationToken cancellationToken);
}

public class MyWorker : IWorker
{
    private readonly ILogger<MyWorker> logger;
    private int number = 0;

    public MyWorker(ILogger<MyWorker> logger)
    {
        this.logger = logger;
    }

    public async Task DoWork(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            Interlocked.Increment(ref number);
            logger.LogInformation($"Worker printing number: {number}");
            await Task.Delay(1000 * 5);
        }
    }
}
