namespace TransactionStore.API.Models.InputModels
{
    public class TransactionEntityInputModel
    {
        public decimal Amount { get; set; }
        public AccountInputModel Account { get; set; }
        public string TimeStamp { get; set; }
    }
}
