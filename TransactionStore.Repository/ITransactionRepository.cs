using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.DB.Models;

namespace TransactionStore.Repository
{
    public interface ITransactionRepository
    {
        ValueTask<RequestResult<TransactionEntity>> DepositOrWithdraw(TransactionEntity dataModel);
        ValueTask<RequestResult<TransactionEntity>> GetTransactionById(int id);
        ValueTask<RequestResult<List<TransactionEntity>>> Transfer(TransferEntities dataModels);
        ValueTask<RequestResult<List<TransactionEntity>>> GetAllTransactions();
        ValueTask<RequestResult<AccountBalance>> GetAccountBalance(int accountId);
        ValueTask<RequestResult<TransactionEntity>> InitializeTransaction(TransactionEntity dataModel);
    }
}