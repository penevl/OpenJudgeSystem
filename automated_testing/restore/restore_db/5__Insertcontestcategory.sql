USE [OpenJudgeSystem]
GO
SET IDENTITY_INSERT [dbo].[ContestCategories] ON 

INSERT [dbo].[ContestCategories] ([Id], [Name], [OrderBy], [ParentId], [IsVisible], [CreatedOn], [ModifiedOn], [IsDeleted], [DeletedOn]) VALUES (1, N'Test Category', 0, NULL, 1, CAST(N'2022-07-05T10:45:54.9324031' AS DateTime2), NULL, 0, NULL)
SET IDENTITY_INSERT [dbo].[ContestCategories] OFF
GO
