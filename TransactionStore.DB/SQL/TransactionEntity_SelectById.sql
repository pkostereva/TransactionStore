drop proc if exists dbo.TransactionEntity_SelectById
go

create proc dbo.TransactionEntity_SelectById
	@id int
as
begin
	select 
	te.Id, 
	te.Amount, 
	te.Timestamp, 
	tt.Id, 
	tt.Name,
	te.AccountId as 'Id'
	
	from dbo.TransactionEntity te
	inner join dbo.TransactionType tt on te.TypeId = tt.Id
	
	where te.Id=@id
END;

--exec dbo.TransactionEntity_SelectById 1