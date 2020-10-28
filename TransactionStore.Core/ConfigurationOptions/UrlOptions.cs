namespace TransactionStore.Core.ConfigurationOptions
{
    public class UrlOptions : IUrlOptions
    {
        public string CrmApiUrl { get; set; }
        public string TransactionStoreApiUrl { get; set; }
    }
}
