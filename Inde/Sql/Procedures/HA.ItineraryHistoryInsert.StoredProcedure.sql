USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[ItineraryHistoryInsert]
GO
/****** Object:  StoredProcedure [HA].[ItineraryHistoryInsert]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


--  [HA].[ItineraryHistoryInsert] 61387,'3-1-2025','3-2-2025'


CREATE PROCEDURE [HA].[ItineraryHistoryInsert]
(
	@SmsIntegrationId int,
	@BeginDate DateTime,
	@EndDate DateTime
)
AS

BEGIN

	DECLARE @Ids TABLE (Id int);


	with cte ( icode,rn ) 
	as
	(
		select
			ih.icode,
			ROW_NUMBER() over ( partition by ih.icode order by ic.itcid ) as rn
		from
			[pb_springermiller].[dbo].[IN_ITINH] ih 
			join [pb_springermiller].[dbo].in_itinc ic on ic.icode = ih.icode
			join [pb_springermiller].[dbo].in_res ir on ir.resno = ic.itcid 
		where
			ic.itype = 'R'
			and ir.arrival between  convert(date,  @BeginDate) and convert(date, @EndDate)
	)
	INSERT INTO [HA].[ItineraryHistory]
	(
		[SmsIntegrationId]
		,[icode]
		,[CreateDate]
		,[UpdateDate]
		,[StatusId]
		,StatusMessage
	)
	output 
		inserted.ItineraryHistoryId 
	into 
		@Ids 

	select
		@SmsIntegrationId,
		cte.icode,
		getdate(),
		getdate(),
		1,
		''
	from 
		cte 
	where
		cte.rn = 1
		and not exists ( select 1 from ha.ItineraryHistory hist where hist.icode = cte.icode and hist.SmsIntegrationId = @SmsIntegrationId)

	--union 

	--select
	--	@SmsIntegrationId,
	--	ih.icode,
	--	getdate(),
	--	getdate() ,
	--	1 as StatusId,
	--	'' as StatusMessage
	--from
	--	[pb_springermiller].[dbo].[IN_ITINH] ih 
	--	join [pb_springermiller].[dbo].in_itinc ic on ic.icode = ih.icode
	--	join rs_sked rs on rs.sknum = ic.itcid 
	--where
	--	ic.itype != 'R'
	--	and rs.skdate between  convert(date,  @BeginDate) and convert(date, @EndDate)
	--	and not exists ( select 1 from ha.ItineraryHistory hist where hist.SmsIntegrationId = @SmsIntegrationId and hist.icode = ih.icode )

	SELECT 
		 a.[ItineraryHistoryId]
		,a.[SmsIntegrationId]
		,a.[icode]
		,a.[CreateDate]
		,a.[UpdateDate]
		,a.[StatusId]
		,a.[StatusMessage]
	FROM 
		[HA].[ItineraryHistory] a
		join @Ids i on i.Id = a.ItineraryHistoryId



END
GO
