using ButlerLee.API.Contracts.IServices;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ButlerLee.API.Scheduler
{
    public class CancellingUnpaidReservations : ScheduledProcessor
    {
        public CancellingUnpaidReservations(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        // 매 10초마다
        protected override string Schedule => "*/10 * * * * *";

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                IServiceWrapper service = serviceProvider.GetRequiredService<IServiceWrapper>();
                await service.Reservation.CancelUnpaidReservations();
            }
        }
    }
}
