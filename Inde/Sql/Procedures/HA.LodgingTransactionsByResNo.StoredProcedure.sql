USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LodgingTransactionsByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LodgingTransactionsByResNo]
GO
/****** Object:  StoredProcedure [HA].[LodgingTransactionsByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [HA].[LodgingTransactionsByResNo]
(
	@ReservationID varchar(16)
)
AS

BEGIN

		SELECT
			tnum,
			tdate,
			tcode,
			tdebit,
			tqty,
			tunit,
			[top] AS [TOP]
		FROM
			PB_SpringerMiller.[dbo].in_trn
		WHERE 
			tres = @ReservationID
			AND tcredit = 0
			AND tdebit <> 0
			AND tcode not in ('INVALD','XFD')

END
GO
