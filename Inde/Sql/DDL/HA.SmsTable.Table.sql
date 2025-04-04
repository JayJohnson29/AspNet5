IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[HA].[SmsTable]') AND type in (N'U'))
ALTER TABLE [HA].[SmsTable] DROP CONSTRAINT IF EXISTS [DF__SmsTable__IsHist__2D27B809]
GO
/****** Object:  Table [HA].[SmsTable]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[SmsTable]
GO
/****** Object:  Table [HA].[SmsTable]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[SmsTable](
	[SmsTableId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](32) NOT NULL,
	[Path] [nvarchar](128) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsHistorical] [bit] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [HA].[SmsTable] ADD  DEFAULT ((0)) FOR [IsHistorical]
GO
