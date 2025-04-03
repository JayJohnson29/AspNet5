using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;

namespace Sms.Repository;

public class LodgingUnitRepostitory : ILodgingUnitRepostitory
{
    private readonly ILogger<LodgingUnitRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public LodgingUnitRepostitory(ILogger<LodgingUnitRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LodgingUnit>> GetAsync()
    {
        try
        {
            var queryResult = await _dbContext.LodgingUnits.FromSqlRaw("[HA].[UnitTypeDescriptionsGetAll]").ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<LodgingUnit>();
        }
    }
}
