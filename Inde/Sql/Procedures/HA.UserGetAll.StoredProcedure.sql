USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[UserGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[UserGetAll]
GO
/****** Object:  StoredProcedure [HA].[UserGetAll]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


Create PROCEDURE [HA].[UserGetAll] 

AS
BEGIN

	select 
		trim(isnull(usrcode,'')) as usrcode, 
		trim(isnull(usrname,'')) as usrname 
	from 
		PB_SpringerMiller.dbo.in_user

END
GO
