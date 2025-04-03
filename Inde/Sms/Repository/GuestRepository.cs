using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class GuestRepository : IGuestRepository
{
    private readonly ILogger<GuestRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public GuestRepository(ILogger<GuestRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<Guest>> GetAsync(string guestId)
    {
        try
        {
            var param = new SqlParameter("@GuestNum", SqlDbType.VarChar);
            param.Value = guestId;

            var queryResult = await _dbContext.Guests.FromSqlRaw("[HA].[GuestById] @guestnum", param).ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {

            _logger.LogError(e, "Error getting guest {id}", guestId);
            return new List<Guest>();
        }
    }


}
