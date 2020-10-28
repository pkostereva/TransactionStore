drop proc if exists dbo.Balance_GetByAccountId
go

create proc dbo.Balance_GetByAccountId	
	@AccountId int
as
begin
	select sum(amount) as 'Balance',
	(select top(1) t.Timestamp
	from TransactionEntity t
	where accountId = @AccountId
	order by t.Timestamp desc) as 'Timestamp'
	from TransactionEntity
end