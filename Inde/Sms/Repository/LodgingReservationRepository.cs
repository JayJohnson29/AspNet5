using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class LodgingReservationRepository : ILodgingReservationRepository
{
    private readonly ILogger<LodgingReservationRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public LodgingReservationRepository(ILogger<LodgingReservationRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LodgingReservation>> GetAsync(string reservationId)
    {
        try
        {

            var param = new SqlParameter("@ReservationId", SqlDbType.VarChar);
            param.Value = reservationId;

            var queryResult = await _dbContext.LodgingReservations.FromSqlRaw("HA.LodgingReservationById @ReservationId", param).ToListAsync();
            return queryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error in Get Reservation {reservationId}");
            return new List<LodgingReservation>();
        }

    }
}
