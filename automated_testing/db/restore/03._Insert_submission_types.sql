USE [OpenJudgeSystem]
GO
SET IDENTITY_INSERT [dbo].[SubmissionTypes] ON 

INSERT [dbo].[SubmissionTypes] ([Id], [Name], [IsSelectedByDefault], [ExecutionStrategyType], [CompilerType], [AdditionalCompilerArguments], [Description], [AllowBinaryFilesUpload], [AllowedFileExtensions]) VALUES (1, N'C# Code', 0, 37, 10, NULL, NULL, 0, NULL)
INSERT [dbo].[SubmissionTypes] ([Id], [Name], [IsSelectedByDefault], [ExecutionStrategyType], [CompilerType], [AdditionalCompilerArguments], [Description], [AllowBinaryFilesUpload], [AllowedFileExtensions]) VALUES (2, N'Python code', 0, 9, 0, NULL, NULL, 0, NULL)
SET IDENTITY_INSERT [dbo].[SubmissionTypes] OFF
GO
