using ButlerLee.API.BackgroundService;
using Microsoft.Extensions.DependencyInjection;
using NCrontab;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ButlerLee.API.Scheduler
{
    public abstract class ScheduledProcessor : ScopedProcessor
    {
        private CrontabSchedule _schedule;
        private DateTime _nextRun;
        protected abstract string Schedule { get; }
        public ScheduledProcessor(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
            _schedule = CrontabSchedule.Parse(Schedule, new CrontabSchedule.ParseOptions { IncludingSeconds = true });
            _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            do
            {
                DateTime now = DateTime.UtcNow;
                DateTime nextrun = _schedule.GetNextOccurrence(now);
                if (now > _nextRun)
                {
                    await Process();
                    _nextRun = _schedule.GetNextOccurrence(DateTime.UtcNow);
                }
                await Task.Delay(5000, stoppingToken); //5 seconds delay
            }
            while (!stoppingToken.IsCancellationRequested);
        }
    }
}
