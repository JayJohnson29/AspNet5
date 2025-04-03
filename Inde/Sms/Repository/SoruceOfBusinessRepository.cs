using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Text;

namespace Sms.Repository;

public class SourceOfBusinessRepostitory : ISourceOfBusinessRepostitory
{
    private readonly ILogger<SourceOfBusinessRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public SourceOfBusinessRepostitory(ILogger<SourceOfBusinessRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<SourceOfBusiness>> GetAsync()
    {
        try
        {
            //var command = new StringBuilder();
            //command.Append($"select scode, sdescrip from {dbName}.dbo.in_srce  ");


            var queryResult = await _dbContext.SourcesOfBusiness.FromSqlRaw("HA.SourceGetAll").ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<SourceOfBusiness>();
        }
    }
}
