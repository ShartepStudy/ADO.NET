CREATE PROCEDURE [dbo].[InitData]
	@delete bit = 0,
	@deleted int out
AS
BEGIN
	MERGE INTO [dbo].[Teams] AS Target
	USING ( VALUES
		(N'4b13ccec-c2df-4858-94e1-4488c3dce7af', N'Team A', 2, 3, 5, N'Some URL'),
		(N'67da61cb-7586-48bf-8da5-d393f6f20727', N'Team B', 7, 1, 2, N'Some URL'),
		(N'da36bc84-b984-4554-aaaf-71a2a5c7058f', N'Team C', 6, 3, 3, N'Some URL'),
		(N'31114623-5dd7-467f-926d-b4d56f07077f', N'Team D', 1, 5, 4, N'Some URL')	
	) AS Source ([Id], [Name], [Wins], [Losses], [Draws], [PictureUrl])
	ON Target.[Id] = Source.[Id]
	WHEN NOT MATCHED BY TARGET THEN
	-- Insert new rows
	INSERT ([Id], [Name], [Wins], [Losses], [Draws], [PictureUrl])
	VALUES ([Id], [Name], [Wins], [Losses], [Draws], [PictureUrl]);
	
	IF @delete = 1
	BEGIN
		DELETE FROM [dbo].[Teams] WHERE [Name] = N'Team E';
		SET @deleted = @@ROWCOUNT;
	END
END
RETURN @@ROWCOUNT
