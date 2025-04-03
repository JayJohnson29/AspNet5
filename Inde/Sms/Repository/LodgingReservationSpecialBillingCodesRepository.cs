using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;
using System.Text;

namespace Sms.Repository;

public class LodgingReservationSpecialBillingRepostitory : ILodgingReservationSpecialBillingRepostitory
{
    private readonly ILogger<LodgingReservationSpecialBillingRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public LodgingReservationSpecialBillingRepostitory(ILogger<LodgingReservationSpecialBillingRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LodgingReservationSpecialBilling>> GetAsync(string reservationId)
    {
        try
        {
            var parameters = new List<SqlParameter>();

            var resno = new SqlParameter("@ReservationId", SqlDbType.VarChar);
            resno.Value = reservationId;
            resno.Direction = ParameterDirection.Input;
            parameters.Add(resno);

            var queryResult = await _dbContext.LodgingReservationSpecialBillingCodes.FromSqlRaw("[HA].[SpecialBillingByResNo] @ReservationId", [.. parameters]).ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<LodgingReservationSpecialBilling>();
        }
    }
}
