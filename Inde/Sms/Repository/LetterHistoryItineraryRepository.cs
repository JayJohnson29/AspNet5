using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class LetterHistoryItineraryRepository : ILetterHistoryItineraryRepository
{
    private readonly ILogger<LetterHistoryItineraryRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public LetterHistoryItineraryRepository(ILogger<LetterHistoryItineraryRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LetterHistoryItinerary>> GetAllAsync(int smsIntegrationId)
    {
        try
        {
            var parameters = new List<SqlParameter>();

            var smsIntegrationIdParam = new SqlParameter("@SmsIntegrationId", SqlDbType.Int);
            smsIntegrationIdParam.Value = smsIntegrationId;
            smsIntegrationIdParam.Direction = ParameterDirection.Input;
            parameters.Add(smsIntegrationIdParam);


            var queryResult = await _dbContext.LetterHistoryItineraries.FromSqlRaw("[HA].[LetterHistoryItineraryGetAll] @SmsIntegrationId", parameters.ToArray()).ToListAsync();
            return queryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Get Create Itinerary {SmsIntegrationId}", smsIntegrationId);
            return new List<LetterHistoryItinerary>();
        }

    }
}
