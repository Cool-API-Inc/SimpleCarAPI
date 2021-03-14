-- თუ არ არსებობს ვქმნით საჭირო მონაცემთა ბაზას
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'CAR_API_DB')
	BEGIN
		CREATE DATABASE [CAR_API_DB]
	END

USE CAR_API_DB;

-- თუ არ არსებობს ვქმნით საჭირო ცხრილს
if not exists (select * from sysobjects where name='CAR_INFO' and xtype='U')
	CREATE TABLE CAR_INFO (
		CarID varchar(64) not null, -- მანქანის უნიკალური აიდი
		Brand varchar(32) null, -- ბრენდი
		PYear int null, -- გამოშვების წელი
		CarDesc nvarchar(1024) null, -- მანქანის აღწერა
		ImgUrl varchar(400) null, -- ატვირთული სურათის მისამართი
		Features int null, -- მახასიათებლები (მნიშვნელობათა ნამრავლი)
		Cost float null, -- მანქანის მოწოდებული ღირებულება
		Currency varchar(8) null, -- მოწოდებული ვალუტა
		CostGel float null -- ღირებულება ლარში
	);
