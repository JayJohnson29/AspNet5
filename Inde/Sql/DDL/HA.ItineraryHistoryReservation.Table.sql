/****** Object:  Table [HA].[ItineraryHistoryReservation]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[ItineraryHistoryReservation]
GO
/****** Object:  Table [HA].[ItineraryHistoryReservation]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[ItineraryHistoryReservation](
	[ItineraryHistoryReservationId] [int] IDENTITY(1,1) NOT NULL,
	[ItineraryHistoryId] [int] NOT NULL,
	[ReservationType] [char](1) NOT NULL,
	[ReservationId] [char](6) NOT NULL
) ON [PRIMARY]
GO
