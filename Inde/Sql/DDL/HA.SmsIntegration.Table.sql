IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[SmsIntegration]') AND type in (N'U'))
ALTER TABLE [HA].[SmsIntegration] DROP CONSTRAINT IF EXISTS [DF__SmsIntegr__Statu__420DC656]
GO
/****** Object:  Table [HA].[SmsIntegration]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[SmsIntegration]
GO
/****** Object:  Table [HA].[SmsIntegration]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[SmsIntegration](
	[SmsIntegrationId] [int] IDENTITY(1,1) NOT NULL,
	[SmsIntegrationStartTime] [datetime] NOT NULL,
	[SmsIntegrationEndTime] [datetime] NULL,
	[StatusId] [int] NOT NULL,
	[Comment] [varchar](max) NULL,
	[StageUpdateBeginDate] [datetime] NULL,
	[StageUpdateEndDate] [datetime] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [HA].[SmsIntegration] ADD  DEFAULT ((0)) FOR [StatusId]
GO
