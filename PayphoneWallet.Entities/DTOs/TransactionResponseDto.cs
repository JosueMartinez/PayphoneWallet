using PayphoneWallet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Entities.DTOs
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public int SourceWalletId { get; set; }
        public int DestinationWalletId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public TransactionType TransactionType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
