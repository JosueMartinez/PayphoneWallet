using PayphoneWallet.Entities.enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayphoneWallet.Entities.DTOs
{
    public class CreateTransactionDto
    {
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; }
        public int SourceWalletId { get; set; }
        public int DestinationWalletId { get; set; }
        public string? Comment { get; set; }
    }
}
