SET IDENTITY_INSERT [HA].[ConfigurationProperty] ON 

INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (1, N'UseSmsDr', N'Use the SMS Database Restore Application', N'1', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (3, N'HaArtifactDirectory', N'Directory', N'c:\ryan solutions\host adapter\artifacts\springermiller\dbfiles', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (4, N'SmsSourceDirectory', N'SmsSourceDirectory', N'C:\temp\smsdata\Source', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (5, N'BulkCopyBatchSize', N'BatchSize', N'100000', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (6, N'BulkCopyTimeout', N'BulkCopyTimeout', N'300', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (9, N'StageSchemaName', N'Staging Schema Name', N'stage', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (10, N'StageUpdateYears', N'LoadYears', N'5', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (11, N'StageUpdateDays', N'UpdateDays', N'2', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (12, N'IntegrationSchemaName', N'Integration Schema Name', N'Dbo', 0)
INSERT [HA].[ConfigurationProperty] ([ConfigurationPropertyId], [Name], [Description], [Value], [LookupCodeId]) VALUES (13, N'StageUpdateBeginDateSave', N'StageUpdateBeginDate', N'01-01-2020', 0)
SET IDENTITY_INSERT [HA].[ConfigurationProperty] OFF
GO
