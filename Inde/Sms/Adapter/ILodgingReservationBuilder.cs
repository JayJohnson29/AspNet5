using Sms.Entity;
using Sms.MC;

namespace Sms.Adapter
{
    public interface ILodgingReservationBuilder
    {
        Reservation Reservation { get; }

        List<ReservationNameValue> GetPriceBasis(List<LodgingUnit> unitDescriptions, string unit);
        List<ReservationNameValue> GetRatePlanPolicies(List<LodgingReservationRatePlanPolicy> policies, string code, string name);
        List<ReservationNameValue> GetUnit(List<LodgingUnit> unitDescriptions, string unit);
        string GetUserName(List<User> users, string code);
        void SetBookingUser(List<User> users, string code);
        void SetLastEditUser(List<User> users, string code);
        void SetMarketSegment(List<Misc> miscDescriptions, string code);
        void SetPriceBasis(List<LodgingUnit> unitDescriptions, string unit);
        void SetRatePlanPolicies(List<LodgingReservationRatePlanPolicy> policies, string code, string name);
        void SetSourceOfBusiness(List<SourceOfBusiness> sobList, string rsource);
        void SetUnit(List<LodgingUnit> unitDescriptions, string unit);
        void SetReservationRequests(List<Entity.Misc> miscDescriptions, string featrs);
        void SetReservationSpecialBillingRequests(List<Entity.Misc> miscDescriptions, List<Entity.LodgingReservationSpecialBilling> specialBillingCodes, string special);
        void SetReservationNotes(List<Entity.LodgingReservationNote> reservationNotes, string guestNum, string noteType);

        void SetCustomer(Customer customer);
    }
}