/****** Object:  Table [HA].[LetterHistoryItineraryReservation]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[LetterHistoryItineraryReservation]
GO
/****** Object:  Table [HA].[LetterHistoryItineraryReservation]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[LetterHistoryItineraryReservation](
	[LetterHistoryItineraryReservationId] [int] IDENTITY(1,1) NOT NULL,
	[ReservationType] [char](1) NOT NULL,
	[LetterHistoryItineraryId] [int] NOT NULL,
	[ReservationId] [char](6) NOT NULL
) ON [PRIMARY]
GO
