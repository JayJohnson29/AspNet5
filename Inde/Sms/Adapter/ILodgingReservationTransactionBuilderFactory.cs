using Sms.Entity;
using MC;

namespace Sms.Adapter
{
    public interface ILodgingReservationTransactionBuilderFactory
    {
        LodgingReservationTransactionBuilder Create(AppConfig config, Reservation reservation, List<LodgingReservationTransaction> lodgingReservationTransactions);
    }
}