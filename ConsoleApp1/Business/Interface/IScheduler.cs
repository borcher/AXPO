using System;
using System.Threading.Tasks;

namespace Business.Interface
{
    public interface IScheduler
    {
        /// <summary>
        /// Start running the provided work immediately and then periodically at the provided interval.
        /// This method returns when the scheduler stops (it is a long-running call in the current design).
        /// </summary>
        Task StartPeriodicAsync(Func<Task> work, TimeSpan interval);
    }
}
