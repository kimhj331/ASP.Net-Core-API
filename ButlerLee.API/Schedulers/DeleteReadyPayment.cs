using ButlerLee.API.Contracts.IServices;
using ButlerLee.API.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace ButlerLee.API.Schedulers
{
    //카카오 ready상태에서
    //결제완료 되지 않은 건에 대해 DB에서 삭제
    public class DeleteReadyPayments : ScheduledProcessor
    {
        public DeleteReadyPayments(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {

        }

        //매일 0시, 12시에
        protected override string Schedule => "0 0 0,12 * * *";

        public override async Task ProcessInScope(IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                IServiceWrapper service = serviceProvider.GetRequiredService<IServiceWrapper>();
                await service.Payment.DeleteReadyPayments();
            }
        }
    }
}
