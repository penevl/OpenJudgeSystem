USE [OpenJudgeSystem]
GO
SET ANSI_NULLS, QUOTED_IDENTIFIER ON;
GO
SET IDENTITY_INSERT [dbo].[Contests] ON 

INSERT [dbo].[Contests] ([Id], [Name], [IsVisible], [AutoChangeTestsFeedbackVisibility], [CategoryId], [Type], [Duration], [StartTime], [EndTime], [ContestPassword], [PracticePassword], [NewIpPassword], [PracticeStartTime], [PracticeEndTime], [LimitBetweenSubmissions], [OrderBy], [NumberOfProblemGroups], [Description], [CreatedOn], [ModifiedOn], [IsDeleted], [DeletedOn]) VALUES (1, N'Test Contest 1', 1, 0, 1, 0, NULL, CAST(N'2022-06-01T00:00:00.0000000' AS DateTime2), CAST(N'2024-12-31T00:00:00.0000000' AS DateTime2), NULL, NULL, NULL, NULL, NULL, 0, 0, 0, NULL, CAST(N'2022-07-04T00:00:00.0000000' AS DateTime2), CAST(N'2022-07-04T10:45:43.7594863' AS DateTime2), 0, NULL)
SET IDENTITY_INSERT [dbo].[Contests] OFF
GO
