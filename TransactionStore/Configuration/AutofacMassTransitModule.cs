using Autofac;
using MassTransit;
using TransactionStore.API.MassTransit;

namespace TransactionStore.API.Configuration
{
    public class AutofacMassTransitModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register((context) =>
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.Host("localhost");

                    cfg.ReceiveEndpoint("currencyRates", ec =>
                    {
                        ec.ConfigureConsumer<RatesConsumer>(context);
                    });
                });

                busControl.Start();

                return busControl;
            })
            .SingleInstance()
            .As<IBusControl>()
            .As<IBus>();
        }
    }
}
