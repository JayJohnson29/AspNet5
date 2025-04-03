using Sms.Entity;

namespace Sms.Repository
{
    public interface ILodgingReservationNoteRepostitory
    {
        Task<List<LodgingReservationNote>> GetAsync(string reservationId);
    }
}