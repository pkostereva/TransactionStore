using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.DB;
using TransactionStore.DB.Models;
using TransactionStore.DB.Storages;

namespace TransactionStore.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ITransactionStorage _transactionStorage;

        public TransactionRepository(ITransactionStorage transactionStorage)
        {
            _transactionStorage = transactionStorage;
        }

        public async ValueTask<RequestResult<TransactionEntity>> GetTransactionById(int id)
        {
            var result = new RequestResult<TransactionEntity>();
            try
            {
                result.RequestData = await _transactionStorage.TransactionEntityGetById(id);
                result.IsOkay = true;
                return result;
            }
            catch (Exception ex)
            {
                result.exMessage = ex.Message;
                return result;
            }
        }

        public async ValueTask<RequestResult<TransactionEntity>> InitializeTransaction(TransactionEntity dataModel)
        {
            dataModel.Type = new TransactionType
            {
                Id = (int)TransactionTypeEnum.Deposit
            };
            var result = new RequestResult<TransactionEntity>();
            _transactionStorage.TransactionStart();
            try
            {
                result.RequestData = await _transactionStorage.TransactionEntityInsertFirst(dataModel);
                _transactionStorage.TransactionCommit();
                result.IsOkay = true;
                return result;
            }
            catch (Exception ex)
            {
                _transactionStorage.TransactionRollBack();
                result.exMessage = ex.Message;
                return result;
            }
        }

        public async ValueTask<RequestResult<TransactionEntity>> DepositOrWithdraw(TransactionEntity dataModel)
        {
            if (dataModel.Amount > 0)
            {
                dataModel.Type = new TransactionType
                {
                    Id = (int)TransactionTypeEnum.Deposit
                };
            }
            else
                dataModel.Type = new TransactionType
                {
                    Id = (int)TransactionTypeEnum.Withdraw
                };
            var result = new RequestResult<TransactionEntity>();
            _transactionStorage.TransactionStart();
            try
            {
                result.RequestData = await _transactionStorage.TransactionEntityInsert(dataModel);
                _transactionStorage.TransactionCommit();
                result.IsOkay = true;
                return result;
            }
            catch (Exception ex)
            {
                _transactionStorage.TransactionRollBack();
                result.exMessage = ex.Message;
                return result;
            }
        }

        public async ValueTask<RequestResult<List<TransactionEntity>>> Transfer(TransferEntities dataModels)
        {
            if(dataModels.Recipient.Account.CurrencyId != dataModels.Sender.Account.CurrencyId)
            {
                dataModels.Recipient.Amount = CurrencyConverter.ConvertCurrency(dataModels.Recipient.Amount, 
                                dataModels.Sender.Account.CurrencyId, dataModels.Recipient.Account.CurrencyId);
            }
            dataModels.Recipient.Type = new TransactionType { Id = (int)TransactionTypeEnum.Transfer };
            dataModels.Sender.Type = new TransactionType { Id = (int)TransactionTypeEnum.Transfer };
            var result = new RequestResult<List<TransactionEntity>>() { RequestData = new List<TransactionEntity>()};
            _transactionStorage.TransactionStart();
            try
            {
                result.RequestData.Add(await _transactionStorage.TransactionEntityInsert(dataModels.Recipient));
                result.RequestData.Add(await _transactionStorage.TransactionEntityInsert(dataModels.Sender));
                _transactionStorage.TransactionCommit();
                result.IsOkay = true;
            }
            catch (Exception ex)
            {
                _transactionStorage.TransactionRollBack();
                result.exMessage = ex.Message;
            }
            return result;
        }

        public async ValueTask<RequestResult<List<TransactionEntity>>> GetAllTransactions()
        {
            var result = new RequestResult<List<TransactionEntity>>();
            try
            {
                result.RequestData = await _transactionStorage.TransactionEntityGetAll();
                result.IsOkay = true;
                return result;
            }
            catch (Exception ex)
            {
                result.exMessage = ex.Message;
                return result;
            }
        }

        public async ValueTask<RequestResult<AccountBalance>> GetAccountBalance(int accountId)
        {
            var result = new RequestResult<AccountBalance>();
            try
            {
                result.RequestData = await _transactionStorage.GetAccountBalance(accountId);
                if (result.RequestData == null)
                {
                    result.RequestData = new AccountBalance
                    {
                        AccountId = accountId,
                        Balance = 0
                    };
                }

                result.IsOkay = true;
                return result;
            }
            catch (Exception ex)
            {
                result.exMessage = ex.Message;
                return result;
            }
        }
    }
}
