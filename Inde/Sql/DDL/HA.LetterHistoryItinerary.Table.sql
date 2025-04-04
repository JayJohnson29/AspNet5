/****** Object:  Table [HA].[LetterHistoryItinerary]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[LetterHistoryItinerary]
GO
/****** Object:  Table [HA].[LetterHistoryItinerary]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[LetterHistoryItinerary](
	[LetterHistoryItineraryId] [int] IDENTITY(1,1) NOT NULL,
	[letterhistoryid] [int] NOT NULL,
	[lnum] [varchar](6) NOT NULL,
	[lcode] [varchar](8) NOT NULL,
	[lguestnum] [char](6) NULL,
	[lmod] [char](3) NULL,
	[lacctnum] [char](6) NULL,
	[icode] [char](9) NOT NULL,
	[iguest] [char](6) NULL,
	[icdate] [date] NULL,
	[ictime] [char](5) NULL,
	[iexpdate] [date] NULL,
	[iexptime] [char](5) NULL,
	[iarrive] [date] NULL,
	[idepart] [date] NULL,
	[ibkdprop] [char](6) NULL,
	[icharges] [money] NULL,
	[ipayments] [money] NULL,
	[iconfnum] [char](9) NULL
) ON [PRIMARY]
GO
