SET ANSI_NULLS ON

DROP TABLE IF EXISTS [data]
CREATE TABLE [data]
(
	[id] [int] NOT NULL IDENTITY(1,1),
	[value] [nvarchar](max) NULL,
	[created_at] [datetime2](7) NOT NULL default getdate(),
	[updated_at] [datetime2](7) NOT NULL default getdate(),
	CONSTRAINT [PK_data] PRIMARY KEY CLUSTERED ([id] ASC) WITH (
		STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF
	) ON [PRIMARY]
)
ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

DROP TRIGGER IF EXISTS [trg_data_UpdateUpdatedAt]
GO

CREATE TRIGGER [trg_data_UpdateUpdatedAt] ON [data]
AFTER UPDATE AS
BEGIN
	IF (update([id]) or update([value]) or update([created_at]))
	BEGIN
		UPDATE	[data]
		SET			[updated_at] = GETDATE()
		FROM		[data]
			INNER JOIN [inserted]
			ON		[data].[id] = [inserted].[id]
	END
END
