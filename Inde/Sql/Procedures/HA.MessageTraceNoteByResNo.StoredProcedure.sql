USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[MessageTraceNoteByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[MessageTraceNoteByResNo]
GO
/****** Object:  StoredProcedure [HA].[MessageTraceNoteByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROC [HA].[MessageTraceNoteByResNo]
(
	@ResNo varchar(16)
)
AS
BEGIN
	select 
		msType, 
		msDate, 
		mstxt 
	from 
		PB_SpringerMiller.[dbo].in_msg
	where 
		msrnum = @ResNo
		and msType in ('T')
END
GO
