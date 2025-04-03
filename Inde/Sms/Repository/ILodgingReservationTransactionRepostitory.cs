using Sms.Entity;

namespace Sms.Repository
{
    public interface ILodgingReservationTransactionRepostitory
    {
        Task<List<LodgingReservationTransaction>> GetAsync(string reservationId);
    }
}