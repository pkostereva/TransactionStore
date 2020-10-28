using System;
using System.Collections.Generic;

namespace TransactionStore.Repository
{
    public static class CurrencyConverter
    {
        public static decimal ConvertCurrency(decimal amount, int initialCurrencyId, int targetCurrencyId)
        {
            if (targetCurrencyId == 1)
            {
                var initialCurrencyCode = ((CurrencyEnum)initialCurrencyId).ToString(); 
                amount /= CurrencyRates.ActualCurrencyRates.Find(x => x.Code == initialCurrencyCode).Rate;
            }
            else if (initialCurrencyId == 1)
            {
                var targetCurrencyCode = ((CurrencyEnum)targetCurrencyId).ToString();
                amount *= CurrencyRates.ActualCurrencyRates.Find(x => x.Code == targetCurrencyCode).Rate;
            }
            else
            {
                var initialCurrencyCode = ((CurrencyEnum)initialCurrencyId).ToString();
                var targetCurrencyCode = ((CurrencyEnum)targetCurrencyId).ToString();
                amount *= CurrencyRates.ActualCurrencyRates.Find(x => x.Code == targetCurrencyCode).Rate /
                          CurrencyRates.ActualCurrencyRates.Find(x => x.Code == initialCurrencyCode).Rate;
            }
            return amount;
        }
    }
}
