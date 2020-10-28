namespace TransactionStore.API.Models.InputModels
{
    public class TransferInputModel
    {
        public decimal Amount { get; set; }
        public AccountInputModel SenderAccount { get; set; }
        public AccountInputModel RecipientAccount { get; set; }
        public string SenderTimeStamp { get; set; }
        public string RecipientTimeStamp { get; set; }
    }
}
