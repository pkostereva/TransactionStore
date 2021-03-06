drop proc if exists dbo.TransactionEntity_InsertTest
go

create proc dbo.TransactionEntity_InsertTest
	@TypeId int,
	@Amount money,
	@AccountId bigint,
	@Timestamp datetime2
as
begin
	if (@Timestamp != (select top(1) Timestamp	
	from TransactionEntity 
	where accountId = @AccountId
	order by Timestamp desc))
		--'A check constraint violation occurred.';
		RAISERROR (50001,-1,16);
	else 		
		insert into dbo.TransactionEntity (TypeId, Amount, Timestamp, AccountId)
		values (@TypeId, @Amount, GETDATE(), @AccountId)
		select SCOPE_IDENTITY();
end