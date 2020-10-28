using System;
using System.Collections.Generic;
using System.Text;

namespace TransactionStore.DB.Models
{
    public class AccountBalance
    {
        public long AccountId { get; set; }
        public decimal Balance { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
