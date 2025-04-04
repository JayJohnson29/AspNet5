USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[LetterHistoryInsert]
GO
/****** Object:  StoredProcedure [HA].[LetterHistoryInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [HA].[LetterHistoryInsert]
(
	@SmsIntegrationId int
)
AS

BEGIN

	INSERT INTO [HA].[LetterHistory]
			   (SmsIntegrationId
				,[lnum]
			   ,[CreateDate]
			   ,[UpdateDate]
			   ,[StatusId]
			   ,[StatusMessage])

	select 
		@SmsIntegrationId,
		w.lnum, 
		getdate() as createdate,
		getdate() as updatedate,
		1 as statusId,
		'' as StatusMessage
	from
		PB_SpringerMiller.dbo.in_wmail w 
	where 
		w.ladddt > dateadd( day, -60, getdate())
		and w.lcode in ('IEMAIL', 'REMAIL', 'VEMAIL','DNDEMAIL','UPGDMAIL','TAIEMAIL','TAREMAIL','VEIAGT','IEIAGT','GEMAIL','SPEVMAIL','INSPIRAT','CXTMAIL','CLMAIL')
		and not exists ( select 1 from [inntopia].[HA].[LetterHistory] lh where lh.lnum = w.[lnum] ) 

END
GO
