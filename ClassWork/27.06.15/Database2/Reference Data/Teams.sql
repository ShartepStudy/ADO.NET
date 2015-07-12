MERGE INTO [dbo].[Teams] AS Target
USING (VALUES

    (N'8c5d6d4c-ff65-47bb-92d3-17e710c5c87b', N'Team A', 2, 3, 5, N'Some URL'),
    (N'e3f7bfcb-c850-4056-8cc2-d6bc073f21a4', N'Team B', 7, 1, 2, N'Some URL'),
    (N'578595bc-f18c-4ba3-9fbc-10e8b34075cf', N'Team C', 6, 3, 3, N'Some URL'),
    (N'e75d4311-9479-408e-84d3-38d601765e4c', N'Team D', 1, 5, 4, N'Some URL')

) AS Source ([Id], [Name], [Wins], [Losses], [Draws], [PictureUrl])
ON Target.[Id] = Source.[Id]
WHEN NOT MATCHED BY TARGET THEN
-- Insert new rows
INSERT ([Id], [Name], [Wins], [Losses], [Draws], [PictureUrl])
VALUES ([Id], [Name], [Wins], [Losses], [Draws], [PictureUrl]);
GO
