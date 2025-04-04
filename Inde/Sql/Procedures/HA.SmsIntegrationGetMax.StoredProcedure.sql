USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[SmsIntegrationGetMax]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[SmsIntegrationGetMax]
GO
/****** Object:  StoredProcedure [HA].[SmsIntegrationGetMax]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROC [HA].[SmsIntegrationGetMax]

AS
BEGIN


	SELECT 
		[SmsIntegrationId]
		,[SmsIntegrationStartTime]
		,[SmsIntegrationEndTime]
		,[StatusId]
		,[Comment]
		,[StageUpdateBeginDate]
		,[StageUpdateEndDate]
	FROM 
		[PB_SpringerMiller].ha.[SmsIntegration]
	where
		SmsIntegrationId = (select max(SmsIntegrationId) from [PB_SpringerMiller].HA.SmsIntegration)


END
GO
