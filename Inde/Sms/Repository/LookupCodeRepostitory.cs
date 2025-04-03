using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Text;

namespace Sms.Repository;

public class LookupCodeRepostitory : ILookupCodeRepostitory
{
    private readonly ILogger<LookupCodeRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public LookupCodeRepostitory(ILogger<LookupCodeRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LookupCode>> GetAsync(string dbName)
    {
        try
        {
            var command = new StringBuilder();
            command.Append($"select 'F' as codeType, brcode as code, brdesc as description from {dbName}.dbo.in_misc2 where brfile = 'F' ");
            command.Append($"union ");
            command.Append($"select 'S' as codeType, vcode as code, vdescr as description from {dbName}.dbo.in_misc where vfile = 'S' ");


            var queryResult = await _dbContext.LookupCodes.FromSqlRaw(command.ToString()).ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<LookupCode>();
        }
    }
}
