using System.Collections.Generic;
using TransactionStore.Repository;

namespace TransactionStore.Tests
{
    public static class CurrencyConvertMock
    {
        public static List<ActualCurrency> ActualCurrencyRates = new List<ActualCurrency>()
        {
            new ActualCurrency
            {
                Code = "RUB",
                Rate = (decimal)73.162
            },
            new ActualCurrency
            {
                Code = "EUR",
                Rate = (decimal)0.924
            },
            new ActualCurrency
            {
                Code = "JPY",
                Rate = (decimal)107.149
            }
        };
    }
}
