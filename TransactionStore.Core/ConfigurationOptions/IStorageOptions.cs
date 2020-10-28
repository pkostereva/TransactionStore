namespace TransactionStore.Core.ConfigurationOptions
{
    public interface IStorageOptions
    {
        string DBConnectionString { get; set; }
    }
}