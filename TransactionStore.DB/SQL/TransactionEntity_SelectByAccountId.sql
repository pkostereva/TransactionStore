drop proc if exists dbo.TransactionEntity_SelectByAccountId
go

create proc dbo.TransactionEntity_SelectByAccountId
@AccountId int
as
begin
    select 
		te.Id,
		te.AccountId,
		te.Timestamp,
		te.Amount,
		tt.Id,
		tt.Name
	FROM dbo.TransactionEntity te
	inner join dbo.TransactionType tt on tt.Id = te.TypeId
	where AccountId = @AccountId
end