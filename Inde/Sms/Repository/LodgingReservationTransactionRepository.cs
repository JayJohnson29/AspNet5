using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class LodgingReservationTransactionRepostitory : ILodgingReservationTransactionRepostitory
{
    private readonly ILogger<LodgingReservationTransactionRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public LodgingReservationTransactionRepostitory(ILogger<LodgingReservationTransactionRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LodgingReservationTransaction>> GetAsync(string reservationId)
    {
        try
        {
            var parameters = new List<SqlParameter>();

            var resno = new SqlParameter("@ReservationId", SqlDbType.VarChar);
            resno.Value = reservationId;
            resno.Direction = ParameterDirection.Input;
            parameters.Add(resno);

            var queryResult = await _dbContext.LodgingReservationTransactions.FromSqlRaw("HA.[LodgingTransactionsByResNo] @ReservationId", parameters.ToArray()).ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<LodgingReservationTransaction>();
        }
    }
}
