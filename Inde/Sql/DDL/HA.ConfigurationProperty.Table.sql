/****** Object:  Table [HA].[ConfigurationProperty]    Script Date: 3/28/2025 2:19:35 PM ******/
DROP TABLE IF EXISTS [HA].[ConfigurationProperty]
GO
/****** Object:  Table [HA].[ConfigurationProperty]    Script Date: 3/28/2025 2:19:35 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [HA].[ConfigurationProperty](
	[ConfigurationPropertyId] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Description] [varchar](50) NOT NULL,
	[Value] [varchar](max) NOT NULL,
	[LookupCodeId] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
