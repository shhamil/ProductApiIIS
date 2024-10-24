CREATE DATABASE TestDb

GO

USE [TestDb]

GO

CREATE TABLE Product
(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	Name NVARCHAR(255) UNIQUE NOT NULL,
	Description NVARCHAR(MAX)
)

GO

CREATE TABLE ProductVersion
(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	ProductID UNIQUEIDENTIFIER ,
	Name NVARCHAR(255) NOT NULL,
	Description NVARCHAR(MAX),
	CreatingDate DATETIME DEFAULT GETUTCDATE() NOT NULL,
	Width FLOAT NOT NULL CHECK (Width >= 0),
    Height FLOAT NOT NULL CHECK (Height >= 0),
    Length FLOAT NOT NULL CHECK (Length >= 0),
	FOREIGN KEY (ProductID)  REFERENCES Product (ID) ON DELETE CASCADE
)

GO

CREATE NONCLUSTERED INDEX IX_ProductVersion_Name
ON ProductVersion (Name)
WITH (ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);

GO

CREATE NONCLUSTERED INDEX IX_ProductVersion_CreatingDate
ON ProductVersion (CreatingDate)
WITH (ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);

GO

CREATE NONCLUSTERED INDEX IX_ProductVersion_Width
ON ProductVersion (Width)
WITH (ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);

GO

CREATE NONCLUSTERED INDEX IX_ProductVersion_Height
ON ProductVersion (Height)
WITH (ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);

GO

CREATE NONCLUSTERED INDEX IX_ProductVersion_Length
ON ProductVersion (Length)
WITH (ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);

GO

CREATE TABLE EventLog
(
	ID UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	EventDate DATETIME DEFAULT GETUTCDATE() NOT NULL,
	Description NVARCHAR(MAX)
)

GO

CREATE NONCLUSTERED INDEX IX_ProductVersion_EventDate
ON EventLog (EventDate)
WITH (ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON);

GO

CREATE TRIGGER Product_INSERT
ON Product
AFTER INSERT
AS
INSERT INTO EventLog (Description)
SELECT 'Добавлено изделие ' + Name
FROM INSERTED

GO

CREATE TRIGGER Product_DELETE
ON Product
AFTER DELETE
AS
INSERT INTO EventLog (Description)
SELECT 'Удалено изделие ' + Name
FROM DELETED

GO

CREATE TRIGGER Product_UPDATE
ON Product
AFTER UPDATE
AS
INSERT INTO EventLog (Description)
SELECT 'Обновлено изделие ' + Name
FROM INSERTED

GO

CREATE TRIGGER ProductVersion_INSERT
ON ProductVersion
AFTER INSERT
AS
INSERT INTO EventLog (Description)
SELECT 'Добавлена версия изделия ' + Name
FROM INSERTED

GO

CREATE TRIGGER ProductVersion_DELETE
ON ProductVersion
AFTER DELETE
AS
INSERT INTO EventLog (Description)
SELECT 'Удалена версия изделия ' + Name
FROM DELETED

GO

CREATE TRIGGER ProductVersion_UPDATE
ON ProductVersion
AFTER UPDATE
AS
INSERT INTO EventLog (Description)
SELECT 'Обновлена версия изделия ' + Name
FROM INSERTED

GO

CREATE FUNCTION SearchProductVersion (
    @productName NVARCHAR(255), 
    @productVersionName NVARCHAR(255), 
    @minVolume FLOAT, 
    @maxVolume FLOAT
)
RETURNS TABLE AS
RETURN (
    SELECT pv.ID, p.Name, pv.Name AS ProductVersionName, pv.Width, pv.Length, pv.Height
    FROM Product AS p
    JOIN ProductVersion AS pv ON pv.ProductID = p.ID
    WHERE p.Name LIKE ('%' + @productName + '%')
      AND pv.Name LIKE ('%' + @productVersionName + '%')
      AND (pv.Width * pv.Length * pv.Height) >= @minVolume 
      AND (pv.Width * pv.Length * pv.Height) <= @maxVolume
)

GO


INSERT INTO Product (Name, Description) VALUES
('Первое изделие', 'Описание первого изделия'),
('Второе изделие', 'Описание второго изделия');

GO

INSERT INTO ProductVersion (ProductID, Name, Description, Width, Height, Length) VALUES
((SELECT ID FROM Product WHERE Name = 'Первое изделие'), 'Version 1.0', 'Описание версии 1.0 первого изделия', 10.5, 15.5, 20.0),
((SELECT ID FROM Product WHERE Name = 'Первое изделие'), 'Version 1.1', 'Описание версии 1.1 первого изделия', 11.0, 16.0, 21.0),
((SELECT ID FROM Product WHERE Name = 'Второе изделие'), 'Version 1.0', 'Описание версии 1.0 второго изделия', 12.0, 18.0, 25.0);