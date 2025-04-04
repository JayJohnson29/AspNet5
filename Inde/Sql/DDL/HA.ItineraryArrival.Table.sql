IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[ItineraryArrival]') AND type in (N'U'))
ALTER TABLE [HA].[ItineraryArrival] DROP CONSTRAINT IF EXISTS [DF__Itinerary__Updat__095F58DF]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[ItineraryArrival]') AND type in (N'U'))
ALTER TABLE [HA].[ItineraryArrival] DROP CONSTRAINT IF EXISTS [DF__Itinerary__Creat__086B34A6]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[ItineraryArrival]') AND type in (N'U'))
ALTER TABLE [HA].[ItineraryArrival] DROP CONSTRAINT IF EXISTS [DF__Itinerary__Statu__0777106D]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[ItineraryArrival]') AND type in (N'U'))
ALTER TABLE [HA].[ItineraryArrival] DROP CONSTRAINT IF EXISTS [DF__Itinerary__Statu__0682EC34]
GO
/****** Object:  Table [HA].[ItineraryArrival]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[ItineraryArrival]
GO
/****** Object:  Table [HA].[ItineraryArrival]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[ItineraryArrival](
	[ItineraryArrivalId] [int] IDENTITY(1,1) NOT NULL,
	[SmsIntegrationId] [int] NOT NULL,
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
	[StatusMessage] [varchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [HA].[ItineraryArrival] ADD  DEFAULT ((0)) FOR [StatusId]
GO
ALTER TABLE [HA].[ItineraryArrival] ADD  DEFAULT ('') FOR [StatusMessage]
GO
ALTER TABLE [HA].[ItineraryArrival] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [HA].[ItineraryArrival] ADD  DEFAULT (getdate()) FOR [UpdateDate]
GO
