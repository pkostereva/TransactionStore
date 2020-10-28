using System;

namespace TransactionStore.DB.Models
{
    public class TransactionEntity
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime TimeStamp { get; set; }
        public TransactionType Type { get; set; }
        public Account Account { get; set; }
    }
}
