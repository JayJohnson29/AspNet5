using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class ActivityReservationRepository : IActivityReservationRepository
{
    private readonly ILogger<ActivityReservationRepository> _logger;
    private readonly SmsDbContext _dbContext;

    public ActivityReservationRepository(ILogger<ActivityReservationRepository> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<ActivityReservation>> GetAsync(string activityReservationId)
    {
        try
        {

            var param = new SqlParameter("@ScheduleId", SqlDbType.VarChar);
            param.Value = activityReservationId;

            var queryResult = await _dbContext.ActivityReservations.FromSqlRaw("[HA].[ActivityReservationById] @ScheduleId", param).ToListAsync();
            return queryResult;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error in Get Reservation {activityReservationId}", activityReservationId);
            return new List<ActivityReservation>();
        }

    }
}

