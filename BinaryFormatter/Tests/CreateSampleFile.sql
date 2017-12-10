

DECLARE @DataString VARCHAR(4000) = N'';

;WITH nums AS
(
	SELECT tab.[col] AS [val]
	FROM   (VALUES ('1'), ('2'), ('3'), ('4'), ('5'), ('6'), ('7'), ('8'), ('9'), ('A')) tab(col)
)
SELECT @DataString += (a.[val] + b.[val])
FROM   nums a
CROSS JOIN nums b;

SELECT @DataString AS [SampleDataAsString];

DECLARE @Data VARBINARY(MAX) = CONVERT(VARBINARY(500), @DataString, 2);

SELECT @Data AS [SampleDataAsBinary];

-- The next line requires the Full version of SQL# ( https://SQLsharp.com/ )
SELECT SQL#.File_WriteFileBinary('C:\TEMP\SampleBinary.bin', @Data, N'create', N'');


