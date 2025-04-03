using Sms.Entity;

namespace Sms.Adapter
{
    public interface ILodgingReservationBuilderFactory
    {
        LodgingReservationBuilder Create(LodgingReservation source, AppConfig config, string cenRezId);
    }
}