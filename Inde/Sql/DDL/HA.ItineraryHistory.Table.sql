/****** Object:  Table [HA].[ItineraryHistory]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[ItineraryHistory]
GO
/****** Object:  Table [HA].[ItineraryHistory]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[ItineraryHistory](
	[ItineraryHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[SmsIntegrationId] [int] NOT NULL,
	[icode] [varchar](9) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
	[StatusMessage] [varchar](256) NOT NULL
) ON [PRIMARY]
GO
