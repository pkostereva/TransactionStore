namespace TransactionStore.DB.Models
{
    public class TransferEntities
    {
        public TransactionEntity Sender { get; set; }
        public TransactionEntity Recipient { get; set; }
    }
}
