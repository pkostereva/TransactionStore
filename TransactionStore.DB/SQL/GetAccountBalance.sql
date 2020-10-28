drop proc if exists dbo.GetAccountBalance
go

create proc dbo.GetAccountBalance	
	@AccountId int
as
begin
	select t.AccountId, sum(t.Amount)  as Balance, 
		(select top(1) t2.Timestamp 
		from TransactionEntity as t2
		where t2.AccountId = t.AccountId
		order by t2.AccountId desc) as TimeStamp
	from dbo.TransactionEntity as t
	group by t.AccountId
end