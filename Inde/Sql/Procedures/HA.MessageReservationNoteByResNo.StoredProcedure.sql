USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[MessageReservationNoteByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[MessageReservationNoteByResNo]
GO
/****** Object:  StoredProcedure [HA].[MessageReservationNoteByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [HA].[MessageReservationNoteByResNo] 
(
	@ResNo varchar(10)
)
AS
BEGIN

	SELECT top 1
		im.msrnum,
		im.mstype,
		trim(isnull(im.mstxt,'')) as mstxt,
		trim(isnull(im.msuser,'')) as msuser,
		CONVERT(datetime,CONVERT(varchar(10),im.msdate, 120) + ' ' + im.mstime) AS msdatetime
	FROM 
		PB_SpringerMiller.[dbo].in_msg im
	WHERE 
		im.msrnum = @ResNo
		AND im.mstype = 'N'
	order by
		5 desc
END
GO
