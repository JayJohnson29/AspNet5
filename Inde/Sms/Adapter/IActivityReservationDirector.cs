namespace Sms.Adapter
{
    public interface IActivityReservationDirector
    {
        Task<MC.Reservation> Run(AppConfig config, string itineraryId, string reservationId);

    }
}