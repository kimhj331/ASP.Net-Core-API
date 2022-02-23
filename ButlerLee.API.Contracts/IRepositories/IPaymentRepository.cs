using ButlerLee.API.Models;
using ButlerLee.API.Models.Filters;
using ButlerLee.API.Models.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IRepositories
{
    public interface IPaymentRepository : IRepositoryBase<Entities.Payment>
    {
        Task<PagedList<Entities.Payment>> GetPayments(PaymentParameters parameters);

        Task<Entities.Payment> GetPaymentById(uint id);

        Task<Entities.Payment> GetPaymentByReservationId(int reservationId);

        Task<Entities.Payment> GetPaymentByReservationNo(string reservationNo);

        Task DeleteReadyPayments();

        Task<string> GenerateReservationNo();

        Task<Entities.Payment> UpsertPayment(Entities.Payment payment);
    }
}
