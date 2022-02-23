using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace ButlerLee.API.Contracts.IRepositories
{
    public interface IRepositoryWrapper
    {
        IUnpaidReservationRepository UnpaidReservation { get; }
        IPaymentRepository Payment { get; }
        Task SaveChangesAsync();
    }
}

