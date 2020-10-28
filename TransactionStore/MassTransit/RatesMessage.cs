using System.Collections.Generic;
using TransactionStore.Repository;

namespace Messages
{
    public class RatesMessage
    {
        public List<ActualCurrency> ActualCurrencyRates { get; set; }
    }
}
