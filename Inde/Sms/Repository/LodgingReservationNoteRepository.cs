﻿using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sms.Entity;
using System.Data;

namespace Sms.Repository;

public class LodgingReservationNoteRepostitory : ILodgingReservationNoteRepostitory
{
    private readonly ILogger<LodgingReservationNoteRepostitory> _logger;
    private readonly SmsDbContext _dbContext;

    public LodgingReservationNoteRepostitory(ILogger<LodgingReservationNoteRepostitory> logger, SmsDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task<List<LodgingReservationNote>> GetAsync(string reservationId)
    {
        try
        {
            var parameters = new List<SqlParameter>();

            var resno = new SqlParameter("@ResNo", SqlDbType.VarChar);
            resno.Value = reservationId;
            resno.Direction = ParameterDirection.Input;
            parameters.Add(resno);

            var queryResult = await _dbContext.LodgingReservationNotes.FromSqlRaw("[HA].[MessageReservationNoteByResNo] @ResNo", parameters.ToArray()).ToListAsync();
            return queryResult;

        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting lookup codes");
            return new List<LodgingReservationNote>();
        }
    }
}
