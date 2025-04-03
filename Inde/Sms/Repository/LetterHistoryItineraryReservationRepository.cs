using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class LetterHistoryItineraryReservationRepository : ILetterHistoryItineraryReservationRepository
{
    private readonly ILogger<LetterHistoryItineraryReservationRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public LetterHistoryItineraryReservationRepository(ILogger<LetterHistoryItineraryReservationRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LetterHistoryItineraryReservation>> GetAllAsync(LetterHistoryItinerary itinerary)
    {

        try
        {
            var parameters = new List<SqlParameter>();

            var smsIntegrationIdParam = new SqlParameter("@LetterHistoryItineraryId", SqlDbType.Int);
            smsIntegrationIdParam.Value = itinerary.LetterHistoryItineraryId;
            smsIntegrationIdParam.Direction = ParameterDirection.Input;
            parameters.Add(smsIntegrationIdParam);


            var queryResult = await _dbContext.LetterHistoryItineraryReservations.FromSqlRaw("[HA].[LetterHistoryItineraryReservationGetByLtrHstItineraryId] @LetterHistoryItineraryId", parameters.ToArray()).ToListAsync();
            return queryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Get Create Itinerary {id}", itinerary.LetterHistoryItineraryId);
            return new List<LetterHistoryItineraryReservation>();
        }

    }
}
