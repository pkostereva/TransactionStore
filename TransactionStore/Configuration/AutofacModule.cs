using Autofac;
using TransactionStore.API.Controllers;
using TransactionStore.API.Secure;
using TransactionStore.DB.Storages;
using TransactionStore.Repository;

namespace TransactionStore.API
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TransactionStorage>().As<ITransactionStorage>();
            builder.RegisterType<TransactionRepository>().As<ITransactionRepository>();
            builder.RegisterType<Messages.RatesMessage>().SingleInstance();
            builder.RegisterType<CreatingIPWhiteList>()
                 .OnActivating(c=>c.Instance.GetWhiteIPs()).AutoActivate();
        }
    }
}
