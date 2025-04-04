USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[MiscDescriptionsGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[MiscDescriptionsGetAll]
GO
/****** Object:  StoredProcedure [HA].[MiscDescriptionsGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [HA].[MiscDescriptionsGetAll]
AS

BEGIN


	select
		1 as MiscTypeId,
		convert(varchar(4), trim(isnull([vfile],''))) as [File],
		convert(varchar(4), trim(isnull([vcode],''))) as Code,
		convert(varchar(32),trim(isnull([vdescr],''))) as Description
	from 							   
		pb_springermiller.dbo.in_misc

	union

	select
		2 as MiscTypeId,
		convert(varchar(4), trim(isnull([brfile],''))) as [File],
		convert(varchar(4), trim(isnull([brcode],''))) as Code,
		convert(varchar(32),trim(isnull([brdesc],''))) as Description
	from 
		pb_springermiller.dbo.in_misc2


	union

	select
		3 as MiscTypeId,
		convert(varchar(4), trim(isnull([brfile],''))) as [File],
		convert(varchar(4), trim(isnull([brcode],''))) as Code,
		convert(varchar(32),trim(isnull([brdesc],''))) as Description
	from 
		pb_springermiller.dbo.in_misc3




END
GO
