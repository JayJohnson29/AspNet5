USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[SourceGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[SourceGetAll]
GO
/****** Object:  StoredProcedure [HA].[SourceGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [HA].[SourceGetAll]
AS

BEGIN


	select 
		scode, 
		sdescrip 
	from 
		PB_SpringerMiller.dbo.in_srce

END
GO
