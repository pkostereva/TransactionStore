namespace TransactionStore.Core.ConfigurationOptions
{
    public interface IUrlOptions
    {
        string CrmApiUrl { get; set; }
        string TransactionStoreApiUrl { get; set; }
    }
}