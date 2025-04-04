USE [Inntopia]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[LetterRequest]') AND type in (N'U'))
ALTER TABLE [HA].[LetterRequest] DROP CONSTRAINT IF EXISTS [DF__LetterReq__Updat__66603565]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[LetterRequest]') AND type in (N'U'))
ALTER TABLE [HA].[LetterRequest] DROP CONSTRAINT IF EXISTS [DF__LetterReq__Creat__656C112C]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[LetterRequest]') AND type in (N'U'))
ALTER TABLE [HA].[LetterRequest] DROP CONSTRAINT IF EXISTS [DF__LetterReq__Statu__6477ECF3]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[LetterRequest]') AND type in (N'U'))
ALTER TABLE [HA].[LetterRequest] DROP CONSTRAINT IF EXISTS [DF__LetterReq__Statu__6383C8BA]
GO
/****** Object:  Table [HA].[LetterRequest]    Script Date: 3/29/2025 5:25:19 AM ******/
DROP TABLE IF EXISTS [HA].[LetterRequest]
GO
/****** Object:  Table [HA].[LetterRequest]    Script Date: 3/29/2025 5:25:19 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[LetterRequest](
	[LetterRequestId] [int] IDENTITY(1,1) NOT NULL,
	[SmsIntegrationId] [int] NOT NULL,
	[BeginDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[StatusId] [int] NOT NULL,
	[StatusMessage] [varchar](64) NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [HA].[LetterRequest] ADD  DEFAULT ((0)) FOR [StatusId]
GO
ALTER TABLE [HA].[LetterRequest] ADD  DEFAULT ('') FOR [StatusMessage]
GO
ALTER TABLE [HA].[LetterRequest] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [HA].[LetterRequest] ADD  DEFAULT (getdate()) FOR [UpdateDate]
GO
