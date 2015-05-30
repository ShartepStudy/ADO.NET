CREATE TABLE [dbo].[Games]
(
	[Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(), 
    [Number] INT NOT NULL UNIQUE, 
    [Team1] UNIQUEIDENTIFIER NOT NULL, 
    [Team2] UNIQUEIDENTIFIER NOT NULL, 
    [Result] TINYINT NOT NULL, 
    [Team1BidPercent] TINYINT NOT NULL, 
    [Team2BidPercent] TINYINT NOT NULL, 
    [Date] DATETIME2 NULL, 
    [League] UNIQUEIDENTIFIER NULL, 
    [IsFinished] BIT NOT NULL DEFAULT 0, 
    CONSTRAINT [PK_Games] PRIMARY KEY ([Id]), 
    CONSTRAINT [FK_Games_To_Teams1] FOREIGN KEY ([Team1]) REFERENCES [Teams]([Id]), 
    CONSTRAINT [FK_Games_To_Teams2] FOREIGN KEY ([Team2]) REFERENCES [Teams]([Id]), 
    CONSTRAINT [FK_Games_To_Leagues] FOREIGN KEY ([League]) REFERENCES [Leagues]([Id]),
	CONSTRAINT [CK_Games_Result] CHECK (Result >= 0 AND Result <=2), 
    CONSTRAINT [CK_Games_TeamBids] CHECK ([Team1BidPercent] + [Team2BidPercent] = 100 AND [Team1BidPercent] >= 0 AND [Team2BidPercent] >= 0)
)
