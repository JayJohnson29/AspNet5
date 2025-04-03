using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class LetterRequestRepository : ILetterRequestRepository
{
    private readonly ILogger<LetterRequestRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public LetterRequestRepository(ILogger<LetterRequestRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LetterRequest>> CreateAsync(int smsIntegrationId, DateTime beginDate, DateTime endDate)
    {
        _logger.LogDebug("Begin ItineraryHistoryInsert calling proc [HA].[LetterRequestInsert] {id}", smsIntegrationId);

        try
        {
            var parameters = new List<SqlParameter>();

            var smsIntegrationIdParam = new SqlParameter("@SmsIntegrationId", SqlDbType.Int);
            smsIntegrationIdParam.Value = smsIntegrationId;
            smsIntegrationIdParam.Direction = ParameterDirection.Input;
            parameters.Add(smsIntegrationIdParam);


            var beginDateParam = new SqlParameter("@BeginDate", SqlDbType.DateTime);
            beginDateParam.Value = beginDate;
            beginDateParam.Direction = ParameterDirection.Input;
            parameters.Add(beginDateParam);


            var endDateParam = new SqlParameter("@EndDate", SqlDbType.DateTime);
            endDateParam.Value = endDate;
            endDateParam.Direction = ParameterDirection.Input;
            parameters.Add(endDateParam);

            var queryResult = await _dbContext.LetterRequests.FromSqlRaw("[HA].[LetterRequestInsert] @SmsIntegrationId,@BeginDate, @EndDate", parameters.ToArray()).ToListAsync();
            return queryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Get Create Itinerary {SmsIntegrationId}", smsIntegrationId);
            return new List<LetterRequest>();
        }

    }
}
