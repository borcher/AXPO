using Business.Interface;
using System;
using System.Threading.Tasks;

namespace Business.Process
{
    public class Scheduler : IScheduler
    {
        private readonly ILogger _logger;

        public Scheduler(ILogger logger)
        {
            _logger = logger;
        }

        public async Task StartPeriodicAsync(Func<Task> work, TimeSpan interval)
        {
            // Run first time immediately
            try
            {
                await work().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Scheduler initial run failed: {ex.Message}");
            }

            // Then run periodically
            using (var timer = new PeriodicTimer(interval))
            {
                try
                {
                    while (await timer.WaitForNextTickAsync().ConfigureAwait(false))
                    {
                        try
                        {
                            await work().ConfigureAwait(false);
                        }
                        catch (Exception ex)
                        {
                            _logger?.Error($"Scheduled work failed: {ex.Message}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger?.Info("Scheduler stopped");
                }
            }
        }
    }
}
