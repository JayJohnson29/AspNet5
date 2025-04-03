using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class ItineraryHistoryReservationRepository : IItineraryHistoryReservationRepository
{
    private readonly ILogger<ItineraryHistoryReservationRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public ItineraryHistoryReservationRepository(ILogger<ItineraryHistoryReservationRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<ItineraryHistoryReservation>> CreateAsync(int smsIntegrationId)
    {
        try
        {
            var parameters = new List<SqlParameter>();

            var smsIntegrationIdParam = new SqlParameter("@SmsIntegrationId", SqlDbType.Int);
            smsIntegrationIdParam.Value = smsIntegrationId;
            smsIntegrationIdParam.Direction = ParameterDirection.Input;
            parameters.Add(smsIntegrationIdParam);

            var queryResult = await _dbContext.ItineraryHistoryReservations.FromSqlRaw("[HA].[ItineraryHistoryInsertReservation] @SmsIntegrationId", parameters.ToArray()).ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating ItineraryHistoryReservation SmsIntegrationId {smsIntegrationId}", smsIntegrationId);
            return new List<ItineraryHistoryReservation>();
        }
    }
}
