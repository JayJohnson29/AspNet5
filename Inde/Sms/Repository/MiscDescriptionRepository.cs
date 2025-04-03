using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Text;

namespace Sms.Repository;

public class MiscDescriptionRepository : IMiscDescriptionRepository
{
    private readonly ILogger<MiscDescriptionRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public MiscDescriptionRepository(ILogger<MiscDescriptionRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<Misc>> GetAsync()
    {
        try
        {
            var queryResult = await _dbContext.MiscDescriptions.FromSqlRaw("[HA].[MiscDescriptionsGetAll]").ToListAsync();
            return queryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<Misc>();
        }
    }
}
