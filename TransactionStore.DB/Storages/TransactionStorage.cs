using Dapper;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TransactionStore.DB.Models;
using TransactionStore.Core.ConfigurationOptions;

namespace TransactionStore.DB.Storages
{
    public class TransactionStorage : ITransactionStorage
    {
        private readonly IDbConnection _connection;

        private IDbTransaction _transaction;

        public TransactionStorage(IOptions<StorageOptions> storageOptions)
        {
            _connection = new SqlConnection(storageOptions.Value.DBConnectionString);
        }

        public void TransactionStart()
        {
            _connection.Open();
            _transaction = this._connection.BeginTransaction();
        }

        public void TransactionCommit()
        {
            this._transaction?.Commit();
            this._connection.Close();
        }

        public void TransactionRollBack()
        {
            this._transaction?.Rollback();
            this._connection.Close();
        }

        internal static class SpName
        {
            public const string TransactionEntityInsert = "TransactionEntity_InsertTest";
            public const string TransactionEntityGetById = "TransactionEntity_SelectById";
            public const string TransactionEntityGettByLeadId = "TransactionEntity_SelectByLeadId";
            public const string TransactionEntitySelectAll = "TransactionEntity_SelectAll";
            public const string GetAccountBalance = "Balance_GetByAccountId";
            public const string TransactionEntityInsertFirst = "TransactionEntity_InsertFirst";
        }

        public async ValueTask<TransactionEntity> TransactionEntityInsert(TransactionEntity dataModel)
        {
            var accountId = dataModel.Account.Id;
            var result = await _connection.QueryAsync<long>(
                SpName.TransactionEntityInsert,
                new
                {
                    TypeId = dataModel.Type.Id,
                    dataModel.Amount,
                    accountId,
                    dataModel.TimeStamp
                },
                transaction: _transaction,
                commandType: CommandType.StoredProcedure);
            dataModel.Id = result.FirstOrDefault();
            return await TransactionEntityGetById(dataModel.Id);
        }

        public async ValueTask<TransactionEntity> TransactionEntityInsertFirst(TransactionEntity dataModel)
        {
            var result = await _connection.QueryAsync<long>(
                SpName.TransactionEntityInsertFirst,
                new
                {
                    accountId = dataModel.Account.Id,
                    TypeId = dataModel.Type.Id,
                    dataModel.Amount
                },
                transaction: _transaction,
                commandType: CommandType.StoredProcedure);
            dataModel.Id = result.FirstOrDefault();
            return await TransactionEntityGetById(dataModel.Id);
        }

        public async ValueTask<TransactionEntity> TransactionEntityGetById(long id)
        {
            var result = await _connection.QueryAsync<TransactionEntity, TransactionType, Account, TransactionEntity>(
               SpName.TransactionEntityGetById,
               (transaction, transactionType, account) =>
               {
                   transaction.Account = account;
                   transaction.Type = transactionType;
                   return transaction;
               },
               new { id },
               transaction: _transaction,
               commandType: CommandType.StoredProcedure,
               splitOn: "Id");
            return result.FirstOrDefault();
        }

        public async ValueTask<List<TransactionEntity>> TransactionEntityGetAll()
        {
            var result = await _connection.QueryAsync<TransactionEntity, TransactionType, TransactionEntity>(
                SpName.TransactionEntitySelectAll,
                (transaction, transactionType) =>
                {
                    TransactionEntity transactionEntity = transaction;
                    transactionEntity.Type = transactionType;
                    return transactionEntity;
                },
                param: null,
                transaction: _transaction,
                commandType: CommandType.StoredProcedure,
                splitOn: "Id");
            return result.ToList();
        }

        public async ValueTask<AccountBalance> GetAccountBalance(int accountId)
        {
            var result = await _connection.QueryAsync<AccountBalance>(
                SpName.GetAccountBalance,
                param: new { accountId },
                transaction: _transaction,
                commandType: CommandType.StoredProcedure);
            return result.FirstOrDefault();
        }
    }
}

