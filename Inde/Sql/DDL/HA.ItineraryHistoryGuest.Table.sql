/****** Object:  Table [HA].[ItineraryHistoryGuest]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[ItineraryHistoryGuest]
GO
/****** Object:  Table [HA].[ItineraryHistoryGuest]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[ItineraryHistoryGuest](
	[ItineraryHistoryGuestId] [int] IDENTITY(1,1) NOT NULL,
	[ItineraryHistoryId] [int] NOT NULL,
	[GuestNum] [char](6) NOT NULL
) ON [PRIMARY]
GO
