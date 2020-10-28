drop proc if exists dbo.TransactionEntity_InsertFirst
go

create  proc dbo.TransactionEntity_InsertFirst
	@TypeId int, 
	@AccountId bigint,
	@Amount money
as
begin
	insert into dbo.TransactionEntity (TypeId, Amount, Timestamp, AccountId)
	values (@TypeId, @Amount, GETDATE(), @AccountId)
	select SCOPE_IDENTITY()
end