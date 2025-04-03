using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Text;

namespace Sms.Repository;

public class LodgingReservationRatePlanPolicyRepostitory : ILodgingReservationRatePlanPolicyRepostitory
{
    private readonly ILogger<LodgingReservationRatePlanPolicyRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public LodgingReservationRatePlanPolicyRepostitory(ILogger<LodgingReservationRatePlanPolicyRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LodgingReservationRatePlanPolicy>> GetAsync()
    {
        try
        {
            //var command = new StringBuilder();
            //command.Append($"IF EXISTS (SELECT 1 FROM {dbName}.dbo.sysobjects WHERE name = 'IN_POLIC' and type='U') ");
            //command.Append($"Begin ");
            //command.Append($"select  ip.plcod, isnull(ip.[pltxt1],''), isnull(ip.[pltxt2],''), isnull(ip.[pltxt3],''), isnull(ip.[pltxt4],'') from {dbName}.dbo.in_polic ip ");
            //command.Append($"End");

            var queryResult = await _dbContext.LodgingReservationRatePlanPolicies.FromSqlRaw("[HA].[PolicyGetAll]").ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<LodgingReservationRatePlanPolicy>();
        }
    }
}
