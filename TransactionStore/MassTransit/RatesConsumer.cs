using MassTransit;
using Messages;
using System;
using System.Threading.Tasks;
using TransactionStore.Repository;

namespace TransactionStore.API.MassTransit
{
    public class RatesConsumer : IConsumer<RatesMessage>
    {
        public async Task Consume(ConsumeContext<RatesMessage> context)
        {
            await Console.Out.WriteLineAsync($"Rates recieved: {context.Message.ActualCurrencyRates}");
            CurrencyRates.ActualCurrencyRates = context.Message.ActualCurrencyRates;
        }
    }
}

