using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class ItineraryHistoryGuestRepository : IItineraryHistoryGuestRepository
{
    private readonly ILogger<ItineraryHistoryGuestRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public ItineraryHistoryGuestRepository(ILogger<ItineraryHistoryGuestRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<ItineraryHistoryGuest>> CreateAsync(int smsIntegrationId)
    {
        try
        {
            var parameters = new List<SqlParameter>();

            var smsIntegrationIdParam = new SqlParameter("@SmsIntegrationId", SqlDbType.Int);
            smsIntegrationIdParam.Value = smsIntegrationId;
            smsIntegrationIdParam.Direction = ParameterDirection.Input;
            parameters.Add(smsIntegrationIdParam);

            var queryResult = await _dbContext.ItineraryHistoryGuests.FromSqlRaw("[HA].[ItineraryHistoryInsertGuest] @SmsIntegrationId", parameters.ToArray()).ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error with Itinerary History Insert Guests {smsIntegrationid}", smsIntegrationId);
            return new List<ItineraryHistoryGuest>();
        }
    }
}
