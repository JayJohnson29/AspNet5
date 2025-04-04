USE [Inntopia]
GO
/****** Object:  StoredProcedure [HA].[SpecialBillingByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
DROP PROCEDURE IF EXISTS [HA].[SpecialBillingByResNo]
GO
/****** Object:  StoredProcedure [HA].[SpecialBillingByResNo]    Script Date: 4/3/2025 3:55:10 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [HA].[SpecialBillingByResNo]
(
	@ReservationID varchar(16)
)
AS

BEGIN

	select 
		trim(isnull(sres,'')) as sres,
		trim(isnull(scod,'')) as scod,
		trim(isnull(sdes,'')) as sdes,
		trim(isnull(styp,'')) as styp
	from 					
		PB_SpringerMiller.dbo.in_spbl 
	where 
		sres = @ReservationID

END
GO
