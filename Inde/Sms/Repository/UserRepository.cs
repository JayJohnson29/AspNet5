using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Text;

namespace Sms.Repository;

public class UserRepostitory : IUserRepostitory
{
    private readonly ILogger<UserRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public UserRepostitory(ILogger<UserRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<User>> GetAsync()
    {
        try
        {
            //var command = new StringBuilder();
            //command.Append($"select trim(isnull(usrcode,'')) as usrcode, trim(isnull(usrname,'')) from {dbName}.dbo.in_user  ");


            var queryResult = await _dbContext.Users.FromSqlRaw("[HA].[UserGetAll]").ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<User>();
        }
    }
}
