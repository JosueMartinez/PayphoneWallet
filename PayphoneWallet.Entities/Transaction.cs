using PayphoneWallet.Entities.enums;

namespace PayphoneWallet.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int SourceWalletId { get; set; } 
        public int DestinationWalletId { get; set; }
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Comment { get; set; }

        public Wallet SourceWallet { get; set; }
        public Wallet DestinationWallet { get; set; }
    }
}