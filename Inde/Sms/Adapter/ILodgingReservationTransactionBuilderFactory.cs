using Sms.Entity;
using Sms.MC;

namespace Sms.Adapter
{
    public interface ILodgingReservationTransactionBuilderFactory
    {
        LodgingReservationTransactionBuilder Create(AppConfig config, Reservation reservation, List<LodgingReservationTransaction> lodgingReservationTransactions);
    }
}