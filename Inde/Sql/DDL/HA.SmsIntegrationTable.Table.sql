IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[SmsIntegrationTable]') AND type in (N'U'))
ALTER TABLE [HA].[SmsIntegrationTable] DROP CONSTRAINT IF EXISTS [DF__SmsIntegr__Proce__2A4B4B5E]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[SmsIntegrationTable]') AND type in (N'U'))
ALTER TABLE [HA].[SmsIntegrationTable] DROP CONSTRAINT IF EXISTS [DF__SmsIntegr__SqlTa__29572725]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[SmsIntegrationTable]') AND type in (N'U'))
ALTER TABLE [HA].[SmsIntegrationTable] DROP CONSTRAINT IF EXISTS [DF__SmsIntegr__VfpIn__286302EC]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[SmsIntegrationTable]') AND type in (N'U'))
ALTER TABLE [HA].[SmsIntegrationTable] DROP CONSTRAINT IF EXISTS [DF__SmsIntegr__VfpRe__276EDEB3]
GO
/****** Object:  Table [HA].[SmsIntegrationTable]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[SmsIntegrationTable]
GO
/****** Object:  Table [HA].[SmsIntegrationTable]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[SmsIntegrationTable](
	[SmsIntegrationTableId] [int] IDENTITY(1,1) NOT NULL,
	[SmsIntegrationId] [int] NOT NULL,
	[SmsTableId] [int] NOT NULL,
	[VfpRecordCount] [int] NULL,
	[VfpInitSeconds] [int] NULL,
	[SqlTableRecordCount] [int] NULL,
	[ProcessStartTime] [datetime] NULL,
	[ProcessEndTime] [datetime] NULL,
	[ProcessStatusId] [int] NOT NULL,
	[ProcessComment] [varchar](max) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [HA].[SmsIntegrationTable] ADD  DEFAULT ((0)) FOR [VfpRecordCount]
GO
ALTER TABLE [HA].[SmsIntegrationTable] ADD  DEFAULT ((0)) FOR [VfpInitSeconds]
GO
ALTER TABLE [HA].[SmsIntegrationTable] ADD  DEFAULT ((0)) FOR [SqlTableRecordCount]
GO
ALTER TABLE [HA].[SmsIntegrationTable] ADD  DEFAULT ((0)) FOR [ProcessStatusId]
GO
