DROP TABLE IF EXISTS [Person];
CREATE TABLE [Person]
(
	[Id] INT IDENTITY(1, 1) NOT NULL,
	[Name] NVARCHAR(MAX) NOT NULL,
	[Age] INT NOT NULL,
	[IsMale] BIT NULL
);

INSERT INTO	[Person]
	([Name], [Age], [IsMale])
VALUES
	(N'Bob', 20, 1),
	(N'Alice', 22, 0),
	(N'John', 25, NULL)
;