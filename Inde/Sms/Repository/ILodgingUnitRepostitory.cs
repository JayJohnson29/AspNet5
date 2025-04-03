using Sms.Entity;

namespace Sms.Repository
{
    public interface ILodgingUnitRepostitory
    {
        Task<List<LodgingUnit>> GetAsync();
    }
}