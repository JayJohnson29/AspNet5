using Sms.Entity;

namespace Sms.Adapter
{
    public interface IActivityReservationBuilderFactory
    {
        ActivityReservationBuilder Create(AppConfig config, string itineraryId, ActivityReservation source);
    }
}