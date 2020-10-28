namespace TransactionStore.API.Models.OutputModels
{
    public class TransactionEntityOutputModel
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string TimeStamp { get; set; }
        public long AccountId { get; set; }
    }
}
