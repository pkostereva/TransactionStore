using System.Collections.Generic;
using System.Threading.Tasks;
using TransactionStore.DB.Models;

namespace TransactionStore.DB.Storages
{
    public interface ITransactionStorage
    {
        ValueTask<TransactionEntity> TransactionEntityGetById(long id);
        ValueTask<TransactionEntity> TransactionEntityInsert(TransactionEntity dataModel);
        ValueTask<List<TransactionEntity>> TransactionEntityGetAll();
        ValueTask<AccountBalance> GetAccountBalance(int accountId);
        ValueTask<TransactionEntity> TransactionEntityInsertFirst(TransactionEntity dataModel);
        void TransactionCommit();
        void TransactionRollBack();
        void TransactionStart();
    }
}