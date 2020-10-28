drop proc if exists dbo.TransactionEntity_SelectAll
go

create proc dbo.TransactionEntity_SelectAll
as
begin
    select 
		te.Id, 
		te.Amount, 
		te.Timestamp, 
		te.AccountId, 
		tt.Id, 
		tt.Name
	from dbo.TransactionEntity te
	inner join dbo.TransactionType tt on te.TypeId = tt.Id
end
