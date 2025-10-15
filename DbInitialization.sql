DROP TABLE IF EXISTS dbo.Tasks;

CREATE TABLE dbo.Tasks
(
    Id int PRIMARY KEY IDENTITY,
    Title NVARCHAR(100),
    Description NVARCHAR(100),
    IsCompleted bit,
    CreatedAt DATETIME
);