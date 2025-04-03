using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;

namespace Sms.Repository;

public class SmsIntegrationRepostitory : ISmsIntegrationRepostitory
{

    private readonly ILogger<SmsIntegrationRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public SmsIntegrationRepostitory(ILogger<SmsIntegrationRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<SmsIntegration>> GetAsync()
    {
        try
        {
            var queryResult = await _dbContext.SmsIntegrations.FromSqlRaw("[HA].[SmsIntegrationGetMax]").ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<SmsIntegration>();
        }
    }
}